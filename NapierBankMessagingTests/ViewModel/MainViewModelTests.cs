using Moq;
using NapierBankMessaging.Event;
using NapierBankMessaging.ViewModel;
using NUnit.Framework;
using Prism.Events;

namespace NapierBankMessagingTests.ViewModel
{
    [TestFixture]
    public class MainViewModelTests
    {
        private Mock<OpenMessageConverterWindowEvent> _openMessageConverterWindowEventMock;
        private Mock<IEventAggregator> _eventAggregator;
        private MainViewModel _viewModel;

        [SetUp]
        public void SetUp()
        {
            _openMessageConverterWindowEventMock = new Mock<OpenMessageConverterWindowEvent>();

            _eventAggregator = new Mock<IEventAggregator>();

            _eventAggregator.Setup(x => x.GetEvent<OpenMessageConverterWindowEvent>())
                .Returns(_openMessageConverterWindowEventMock.Object);

            _viewModel = new MainViewModel(_eventAggregator.Object);
        }

        [Test]
        public void OpenMessageConverterWindowCommand_PublishesOpenMessageConverterWindowEvent()
        {
            _viewModel.OpenMessageConverterWindowCommand.Execute(null);

            _openMessageConverterWindowEventMock.Verify(x => x.Publish(), Times.Once);
        }
    }

}
