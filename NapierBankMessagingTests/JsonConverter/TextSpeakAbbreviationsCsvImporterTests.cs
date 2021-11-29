using System;
using System.Collections.Generic;
using System.Text;
using NapierBankMessaging.Import;
using NUnit.Framework;

namespace NapierBankMessagingTests.JsonConverter
{
    [TestFixture]
    public class TextSpeakAbbreviationsCsvImporterTests
    {
        private TextSpeakAbbreviationsCsvImporter _importer;

        [SetUp]
        public void SetUp()
        {
            _importer = new TextSpeakAbbreviationsCsvImporter("..\\..\\..\\Data\\textwords.csv");
        }

        [Test]
        public void Import_DictionaryIsNotEmpty()
        {
            var dict = _importer.ImportTextSpeakAbbreviations();

            foreach (var dictKey in dict.Keys)
            {
                Console.WriteLine(dictKey + " : " + dict[dictKey]);
            }

            Assert.IsTrue(dict.Count > 0);
        }
    }
}
