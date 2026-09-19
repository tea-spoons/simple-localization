
namespace TeaSpoons.SimpleLocalization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This class contains a <see cref="LocalizedString"/> and, optionally, arguments for formatting it.<br/>
    /// <see cref="Apply"/> needs to be called initially and after changing an argument via <see cref="SetArgument"/>.
    /// </summary>
    public class AutoTranslation : IDisposable
    {
        public bool UpdateOnLanguageChange
        {
            get => updateOnLanguageChange;
            set
            {
                if (isDisposed) throw new ObjectDisposedException(nameof(AutoTranslation));
                if (value == updateOnLanguageChange) return;

                updateOnLanguageChange = value;
                if (updateOnLanguageChange)
                {
                    Localization.LanguageUpdated += TranslateAndApply;
                }
                else
                {
                    Localization.LanguageUpdated -= TranslateAndApply;
                }
            }
        }

        private readonly LocalizedString key;
        private string currentTranslation;
        private string currentOutput;
        private readonly Action<string> onApply;
        private readonly Dictionary<string, string> arguments;
        private bool dirty => currentOutput == null;
        private bool isDisposed = false;
        private bool updateOnLanguageChange = false;

        public AutoTranslation(LocalizedString key, Action<string> onApply, bool updateOnLanguageChange)
        {
            this.key = key;
            this.onApply = onApply;

            arguments = LocalizedStringFormatting.GetArgumentDictionary(key);
            currentTranslation = key;

            if (arguments == null)
            {
                Apply();
            }

            UpdateOnLanguageChange = updateOnLanguageChange;
        }

        /// <summary>
        /// Updates the argument with the given <paramref name="key"/> with the given <paramref name="value"/>.
        /// </summary>
        /// <returns>This object, for method chaining.</returns>
        public AutoTranslation SetArgument(string key, object value)
        {
            if (isDisposed) throw new ObjectDisposedException(nameof(AutoTranslation));

            if (arguments != null &&
                arguments.TryGetValue(key, out var previousValue))
            {
                var s = value.ToString();

                if (s != previousValue)
                {
                    arguments[key] = s;
                    SetDirty();
                }
            }

            return this;
        }

        /// <summary>
        /// Returns whether the given arguments correspond with the data inside this instance.
        /// </summary>
        /// <remarks>
        /// Can be used to recycle an <see cref="AutoTranslation"/> instance rather than creating a new one, reducing garbage.
        /// </remarks>
        public bool Matches(LocalizedString key)
        {
            return Equals(key, this.key);
        }

        /// <summary>
        /// Runs the given <c>onApply</c> method with the current translation and the current arguments.
        /// </summary>
        /// <returns>This object, for method chaining.</returns>
        public AutoTranslation Apply()
        {
            if (isDisposed) throw new ObjectDisposedException(nameof(AutoTranslation));

            if (dirty)
            {
                currentOutput = LocalizedStringFormatting.Format(currentTranslation, arguments, true);
            }

            try
            {
                onApply(currentOutput);
            }
            catch (Exception exception)
            {
                UpdateOnLanguageChange = false;
                throw new InvalidOperationException($"An exception was thrown while applying an {nameof(AutoTranslation)}.", exception);
            }

            return this;
        }

        public void Dispose()
        {
            UpdateOnLanguageChange = false;
            isDisposed = true;
        }

        #region System.Object Overrides
        public override bool Equals(object obj)
        {
            return obj is AutoTranslation other &&
                key == other.key;
        }

        public override int GetHashCode()
        {
            return key.GetHashCode();
        }
        #endregion

        private void TranslateAndApply(string key)
        {
            currentTranslation = this.key;
            SetDirty();
            Apply();
        }

        private void SetDirty()
        {
            currentOutput = null;
        }
    }
}
