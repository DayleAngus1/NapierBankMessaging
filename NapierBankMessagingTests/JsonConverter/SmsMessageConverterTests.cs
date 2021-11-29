using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NapierBankMessaging.MessageConvert;
using NapierBankMessaging.Model;
using NUnit.Framework;

namespace NapierBankMessagingTests.JsonConverter
{
    [TestFixture]
    public class SmsMessageConverterTests
    {
        private Mock<ITextSpeakConverter> _textSpeakConverterMock;
        private SmsMessageConverter _messageConverter;
        private string _correctBody;
        private string _textSpeakConverterReturnString;

        [SetUp]
        public void SetUp()
        {
            _textSpeakConverterMock = new Mock<ITextSpeakConverter>();

            _messageConverter = new SmsMessageConverter(_textSpeakConverterMock.Object);

            _correctBody =
                $"+071232123\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque a orci eleifend, dapibus sem quis, dictum nisl. ";

            _textSpeakConverterReturnString = "Abbreviation free message";

            _textSpeakConverterMock.Setup(x => x.ReplaceTextSpeakAbbreviations(It.IsAny<string>()))
                .Returns(_textSpeakConverterReturnString);
        }


        [Test]
        public void ConvertMessage_MessageHasHeader()
        {
            var message = new SmsMessage();
            var header = "Hello World";
            _messageConverter.ConvertMessage(header, _correctBody ,message);

            Assert.AreEqual(header,message.Header);
        }

        [Test]
        public void ConvertMessage_SenderIsSet()
        {
            var message = new SmsMessage();
            var sender = "+071232123";
            _messageConverter.ConvertMessage(It.IsAny<string>(), _correctBody, message);

            Assert.AreEqual(sender, message.Sender);
        }

        [Test]
        public void ConvertMessage_BodyIsTextSpeakConverterReturnText()
        {
            var message = new SmsMessage();
            _messageConverter.ConvertMessage(It.IsAny<string>(), _correctBody, message);

            Assert.AreEqual(_textSpeakConverterReturnString, message.Body);
        }

        [Test]
        public void ConvertMessage_IncorrectSenderThrowsInvalidSenderException()
        {
            var message = new SmsMessage();
            var incorrectBody = "HelloWorld\nHelloWorld";
            Assert.Throws(typeof(InvalidSenderException),
                (() => _messageConverter.ConvertMessage(It.IsAny<string>(), incorrectBody, message)));
        }

        [Test]
        public void ConvertMessage_LongMessageThrowsMessageFormatException()
        {
            var message = new SmsMessage();
            var incorrectBody =
                "+071232123\nLorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque a orci eleifend, dapibus sem quis, dictum nisl. Praesent commodo, ante a pretium iaculis, nisl nisl varius sapien, vitae aliquet sem nulla a ante.";
            Assert.Throws(typeof(MessageFormatException),
                (() => _messageConverter.ConvertMessage(It.IsAny<string>(), incorrectBody, message)));
        }
    }
}
