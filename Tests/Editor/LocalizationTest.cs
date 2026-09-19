
namespace TeaSpoons.SimpleLocalization.Editor.Tests
{
    using NUnit.Framework;
    using System.Collections.Generic;

    public class LocalizationTest
    {
        [SetUp]
        public void SetUp()
        {
            Localization.ResetActiveDictionary();
        }

        [TearDown]
        public void TearDown()
        {
            Localization.ResetActiveDictionary();
        }

        [Test]
        public void Translation()
        {
            var translations = new Dictionary<string, string>
            {
                { "Good morning.", "Guten Morgen." }
            };
            var dictionary = new LocalizationDictionary("de", translations);
            dictionary.SetActive();

            AssertAreEqual("Guten Morgen.", new LocalizedString("Good morning."));
            AssertAreEqual("not found", new LocalizedString("not found"));
        }

        [Test]
        public void StringFormatting()
        {
            Assert.AreEqual("You have 4 apples.", new LocalizedString("You have {{apple count}} apples.").Format(new()
            {
                { "apple count", "4" }
            }));
            Assert.AreEqual("You have 4 apples and 5 bananas.", new LocalizedString("You have {{apple count}} apples and {{banana count}} bananas.").Format(new()
            {
                { "banana count", "5" },
                { "apple count", "4" }
            }));
            Assert.AreEqual("You have 4 apples, 5 bananas and 6 legendary armor pieces.", new LocalizedString("You have {{apple count}} apples, {{banana count}} bananas and {{lap count}} legendary armor pieces.").Format(new()
            {
                { "banana count", "5" },
                { "apple count", "4" },
                { "lap count", "6" }
            }));
            Assert.AreEqual("a b cd 2 1", new LocalizedString("{{0}} {{1}} {{2}} {{4}} {{3}}").Format(new()
            {
                { "0", "a" },
                { "1", "b" },
                { "2", "cd" },
                { "3", "1" },
                { "4", "2" },
            }));

            // Missing key is fine
            Assert.AreEqual("You have  apples and 5 bananas.", new LocalizedString("You have {{apple count}} apples and {{banana count}} bananas.").Format(new()
            {
                { "banana count", "5" }
            }));

            // Single argument overload
            Assert.AreEqual("You have 42 apples.", new LocalizedString("You have {{apple count}} apples.").Format(42));
        }

        private static void AssertAreEqual(string expected, LocalizedString actual)
        {
            Assert.AreEqual(expected, (string)actual);
        }
    }
}
