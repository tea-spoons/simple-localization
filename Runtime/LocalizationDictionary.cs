
namespace TeaSpoons.SimpleLocalization
{
    using System.Collections.Generic;

    /// <summary>
    /// A dictionary containing the translations from the default language to a specific target language.
    /// </summary>
    public class LocalizationDictionary
    {
        public readonly string LanguageKey;
        private readonly Dictionary<string, string> translations;

        public LocalizationDictionary(string languageKey, Dictionary<string, string> translations)
        {
            LanguageKey = languageKey;
            this.translations = translations;
        }

        /// <summary>
        /// Sets this dictionary as the active dictionary,
        /// making its language the language that is being translated to.
        /// </summary>
        public void SetActive()
        {
            Localization.activeDictionary = this;
        }

        internal bool TryTranslate(string key, out string value)
        {
            return translations.TryGetValue(key, out value);
        }
    }
}
