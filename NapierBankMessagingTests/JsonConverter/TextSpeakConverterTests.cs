using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NapierBankMessaging.Import;
using NapierBankMessaging.MessageConvert;
using NUnit.Framework;

namespace NapierBankMessagingTests.JsonConverter
{
    [TestFixture]
    public class TextSpeakConverterTests
    {
        private TextSpeakConverter _textSpeakConverter;
        private Mock<ITextSpeakAbbreviationsImporter> _textSpeakAbbreviationsImporterMock;

        [SetUp]
        public void SetUp()
        {
            _textSpeakAbbreviationsImporterMock = new Mock<ITextSpeakAbbreviationsImporter>();

            _textSpeakAbbreviationsImporterMock.Setup(x => x.ImportTextSpeakAbbreviations()).Returns(
                new Dictionary<string, string>()
                {
                    {"AAP", "Always a pleasure"},
                    {"AAR", "At any rate"},
                    {"AAS", "Alive and smiling"},
                    {"ADN", "Any day now"}
                });
            _textSpeakConverter = new TextSpeakConverter(_textSpeakAbbreviationsImporterMock.Object);
        }

        [TestCase("AAP","AAP <Always a pleasure>")]
        [TestCase("Hello World AAR Hello World", "Hello World AAR <At any rate> Hello World")]
        [TestCase("Hello World ADN Hello World AAS Hello World AAS Hello World",
            "Hello World ADN <Any day now> Hello World AAS <Alive and smiling> Hello World AAS <Alive and smiling> Hello World")]
        public void ReplaceTextSpeakAbbreviations_ReplacesAbbreviationsWithFullSentences(string input, string expected)
        {
            var output = _textSpeakConverter.ReplaceTextSpeakAbbreviations(input);

            Assert.AreEqual(output, expected);
        }

    }
}
