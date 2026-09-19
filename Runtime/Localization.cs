
namespace TeaSpoons.SimpleLocalization
{
    using System;

    public static class Localization
    {
        public static event Action<string> LanguageUpdated = delegate { };

        private static readonly LocalizationDictionary emptyDictionary = new(null, new());
        private static LocalizationDictionary _activeDictionary;
        internal static LocalizationDictionary activeDictionary
        {
            set
            {
                if (_activeDictionary == value) return;

                _activeDictionary = value;
                LanguageUpdated(_activeDictionary?.LanguageKey ?? null);
            }
            get
            {
                return _activeDictionary ?? emptyDictionary;
            }
        }

#if UNITY_EDITOR
#pragma warning disable IDE0051 // Remove unused private members
        [UnityEditor.InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode()
        {
            ResetActiveDictionary();
        }

        internal static void ResetActiveDictionary()
        {
            _activeDictionary = null;
        }
#pragma warning restore IDE0051 // Remove unused private members
#endif
    }
}
