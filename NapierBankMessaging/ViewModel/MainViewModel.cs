using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using NapierBankMessaging.Event;
using Prism.Commands;
using Prism.Events;

namespace NapierBankMessaging.ViewModel
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        public ICommand OpenMessageConverterWindowCommand { get; set; }

        public MainViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            OpenMessageConverterWindowCommand = new DelegateCommand(OnOpenMessageConverterWindowExecute);
        }

        private void OnOpenMessageConverterWindowExecute()
        {
            _eventAggregator.GetEvent<OpenMessageConverterWindowEvent>().Publish();
        }

    }
}
