
namespace TeaSpoons.SimpleLocalization.Editor.Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;

    public class AutoTranslationTest
    {
        private AutoTranslation autoTranslation;

        [SetUp]
        public void SetUp()
        {
            Localization.ResetActiveDictionary();
        }

        [TearDown]
        public void TearDown()
        {
            Localization.ResetActiveDictionary();

            autoTranslation?.Dispose();
        }

        [Test]
        public void AutoTranslation()
        {
            var result = string.Empty;
            autoTranslation = new AutoTranslation(new LocalizedString("Good morning."), s => result = s, true).Apply();
            Assert.AreEqual("Good morning.", result);

            var translations = new Dictionary<string, string>
            {
                { "Good day.", "Guten Tag." },
                { "Good morning.", "Guten Morgen." },
                { "Good evening.", "Guten Abend." }
            };
            var dictionary = new LocalizationDictionary("de", translations);
            dictionary.SetActive();

            Assert.AreEqual("Guten Morgen.", result);
        }

        [Test]
        public void AutoTranslationWithArguments()
        {
            var result = string.Empty;
            autoTranslation = new AutoTranslation(new LocalizedString("You have {{apple count}} apples."), s => result = s, true).Apply();
            Assert.AreEqual("You have  apples.", result);

            autoTranslation.SetArgument("apple count", 10).Apply();
            Assert.AreEqual("You have 10 apples.", result);

            autoTranslation.SetArgument("apple count", 20).Apply();
            Assert.AreEqual("You have 20 apples.", result);

            var translations = new Dictionary<string, string>
            {
                { "Good morning.", "Guten Morgen." },
                { "You have {{apple count}} apples.", "Du hast {{apple count}} Äpfel." }
            };
            var dictionary = new LocalizationDictionary("de", translations);
            dictionary.SetActive();

            Assert.AreEqual("Du hast 20 Äpfel.", result);
        }
    }
}
