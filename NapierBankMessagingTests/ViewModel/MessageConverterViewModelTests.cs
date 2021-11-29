using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using Moq;
using NapierBankMessaging.Event;
using NapierBankMessaging.MessageConvert;
using NapierBankMessaging.Model;
using NapierBankMessaging.ViewModel;
using NUnit.Framework;
using Prism.Events;

namespace NapierBankMessagingTests.ViewModel
{
    [TestFixture]
    public class MessageConverterViewModelTests
    {
        private Mock<OpenMainWindowEvent> _openMainWindowEventMock;
        private Mock<IEventAggregator> _eventAggregator;
        private MessageConverterViewModel _viewModel;
        private Mock<IHeaderValidator> _headerValidator;
        private Mock<MessageConverter> _smsMessageConverterMock;
        private Mock<MessageConverter> _emailMessageConverterMock;
        private Mock<MessageConverter> _tweetMessageConverterMock;
        private Mock<ITextSpeakConverter> _textSpeakConverterMock;
        private Mock<IMessageConverterDictionaryBuilder>_converterDictBuilderMock;
        private Mock<IMessageFactoryDictionaryBuilder> _messageFactoryDictionaryBuilderMock;

        [SetUp]
        public void SetUp()
        {
            _openMainWindowEventMock = new Mock<OpenMainWindowEvent>();

            _eventAggregator = new Mock<IEventAggregator>();

            _eventAggregator.Setup(x => x.GetEvent<OpenMainWindowEvent>()).Returns(_openMainWindowEventMock.Object);

            _smsMessageConverterMock = new Mock<MessageConverter>();

            _emailMessageConverterMock = new Mock<MessageConverter>();

            _tweetMessageConverterMock = new Mock<MessageConverter>();

            _textSpeakConverterMock = new Mock<ITextSpeakConverter>();
            _converterDictBuilderMock = new Mock<IMessageConverterDictionaryBuilder>();

            _converterDictBuilderMock.Setup(x => x.Build()).Returns(new Dictionary<string, MessageConverter>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"S", _smsMessageConverterMock.Object},
                {"E", _emailMessageConverterMock.Object},
                {"T", _tweetMessageConverterMock.Object}
            });

            _messageFactoryDictionaryBuilderMock = new Mock<IMessageFactoryDictionaryBuilder>();

            _messageFactoryDictionaryBuilderMock.Setup(x => x.Build()).Returns(new Dictionary<string, IMessageFactory>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"S",new SmsMessageFactory()},
                {"E", new EmailMessageFactory()},
                {"T",new TweetMessageFactory()}
            });

            _headerValidator = new Mock<IHeaderValidator>();
            _viewModel = new MessageConverterViewModel(_eventAggregator.Object, _headerValidator.Object, _converterDictBuilderMock.Object, _messageFactoryDictionaryBuilderMock.Object);
        }

        [Test]
        public void OpenMainWindowCommand_PublishesOpenMainWindowEvent()
        {
            _viewModel.OpenMainWindowCommand.Execute(null);

            _openMainWindowEventMock.Verify(x => x.Publish(), Times.Once);
        }

        [Test]
        public void IsValidHeader_TrueIfValidated()
        {
            _headerValidator.Setup(x => x.ValidateHeader(It.IsAny<string>())).Returns(true);

            _viewModel.Header = "S";

            Assert.IsTrue(_viewModel.IsValidHeader);
        }

        [Test]
        public void HeaderSet_CallsHeaderValidator()
        {
            _viewModel.Header = It.IsAny<string>();

            _headerValidator.Verify(x => x.ValidateHeader(It.IsAny<string>()),Times.Once);
        }

        [Test]
        public void HeaderSet_InvalidHeaderSetsErrorMessage()
        {
            _headerValidator.Setup(x => x.ValidateHeader(It.IsAny<string>())).Returns(false);

            _viewModel.Header = It.IsAny<string>();

            Assert.IsNotNull(_viewModel.ErrorMessage);
        }

        [TestCase("s123456789")]
        [TestCase("S123456789")]
        public void BodySet_SmsHeaderCallsSmsConverterConvertMessage(string header)
        {
            _headerValidator.Setup(x => x.ValidateHeader(It.IsAny<string>())).Returns(true);

            _viewModel.Header = header;

            _viewModel.Body = It.IsAny<string>();

            _smsMessageConverterMock.Verify(x => x.ConvertMessage(header,It.IsAny<string>(), It.IsAny<Message>()), Times.Once);
        }

        [TestCase("e123456789")]
        [TestCase("E123456789")]
        public void BodySet_EmailHeaderCallsEmailConverterConvertMessage(string header)
        {
            _headerValidator.Setup(x => x.ValidateHeader(It.IsAny<string>())).Returns(true);

            _viewModel.Header = header;

            _viewModel.Body = It.IsAny<string>();

            _emailMessageConverterMock.Verify(x => x.ConvertMessage(header,It.IsAny<string>(), It.IsAny<Message>()), Times.Once);
        }

        [TestCase("t123456789")]
        [TestCase("T123456789")]
        public void BodySet_TweetHeaderCallsTweetConverterConvertMessage(string header)
        {
            _headerValidator.Setup(x => x.ValidateHeader(It.IsAny<string>())).Returns(true);

            _viewModel.Header = header;

            _viewModel.Body = It.IsAny<string>();

            _tweetMessageConverterMock.Verify(x => x.ConvertMessage(header, It.IsAny<string>(), It.IsAny<Message>()), Times.Once);
        }
    }
}
