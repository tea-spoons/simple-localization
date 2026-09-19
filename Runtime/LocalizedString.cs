
namespace TeaSpoons.SimpleLocalization
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public struct LocalizedString
    {
#if UNITY_EDITOR
        internal static class PropertyNames
        {
            public static string Key => nameof(key);
        }
#endif

        [SerializeField]
        internal string key;

        public LocalizedString(string key)
        {
            this.key = key;
        }

        public readonly bool HasKnownKey()
        {
            return Localization.activeDictionary.TryTranslate(key, out _);
        }

        /// <summary>
        /// Replaces occurrences of "{{something}}" with the value
        /// that has been put into <paramref name="arguments"/> for the key "something".
        /// </summary>
        /// <remarks>
        /// Consider using an <see cref="AutoTranslation"/> instance instead to avoid creating garbage.
        /// </remarks>
        public readonly string Format(Dictionary<string, string> arguments)
        {
            return LocalizedStringFormatting.Format(this, arguments, false);
        }

        /// <summary>
        /// Replaces all occurences of "{{anything}}" with the given <paramref name="argument"/>.
        /// </summary>
        public readonly string Format(object argument)
        {
            return LocalizedStringFormatting.Format(this, argument);
        }

        #region System.Object Overrides
        public override readonly string ToString()
        {
            return $"LocalizedString: \"{key}\"";
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is LocalizedString other)
            {
                return key == other.key;
            }
            return false;
        }

        public override readonly int GetHashCode()
        {
            return key.GetHashCode();
        }
        #endregion

        #region Implicit operators
        public static implicit operator string(LocalizedString localizedString)
        {
            if (Localization.activeDictionary.TryTranslate(localizedString.key, out var value))
            {
                return value;
            }
            return localizedString.key;
        }

        public static implicit operator LocalizedString(string key)
        {
            return new LocalizedString(key);
        }
        #endregion
    }
}
