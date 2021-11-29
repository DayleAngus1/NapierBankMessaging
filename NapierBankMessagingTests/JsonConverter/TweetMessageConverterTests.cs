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
    public class TweetMessageConverterTests
    {
        private TweetMessageConverter _converter;
        private Mock<ITextSpeakConverter> _textSpeakConverter;

        [SetUp]
        public void SetUp()
        {
            _textSpeakConverter = new Mock<ITextSpeakConverter>();            
            _converter = new TweetMessageConverter(_textSpeakConverter.Object);
        }
    }
}
