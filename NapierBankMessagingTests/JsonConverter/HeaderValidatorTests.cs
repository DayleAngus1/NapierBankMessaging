using System;
using System.Collections.Generic;
using System.Text;
using NapierBankMessaging.MessageConvert;
using NUnit.Framework;

namespace NapierBankMessagingTests.JsonConverter
{
    [TestFixture]
    public class HeaderValidatorTests
    {
        [TestCase("S123456789")]
        [TestCase("T123456789")]
        [TestCase("E123456789")]
        [TestCase("S111110000")]
        [TestCase("S000000000")]
        [TestCase("S111111111")]
        [TestCase("S999999999")]
        [TestCase("s123456789")]
        [TestCase("e123456789")]
        public void ValidateHeader_ReturnsTrueForCorrectHeaders(string header)
        {
            var validator = new HeaderValidator();
            Assert.IsTrue(validator.ValidateHeader(header));
        }

        [TestCase("S12345678r")]
        [TestCase("T12345678")]
        [TestCase("w123456789")]
        [TestCase("asdfasssdf")]
        [TestCase("S000000000123")]
        [TestCase("SS11111111")]
        [TestCase(" s123456789")]
        public void ValidateHeader_ReturnsFalseForIncorrectHeaders(string header)
        {
            var validator = new HeaderValidator();
            Assert.IsFalse(validator.ValidateHeader(header));
        }
    }
}
