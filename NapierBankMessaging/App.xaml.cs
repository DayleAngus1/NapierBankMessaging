using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using NapierBankMessaging.Event;
using NapierBankMessaging.Startup;
using Prism.Events;

namespace NapierBankMessaging
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Bootstrapper _bootstrapper;
        private IContainer _container;
        private MainWindow _mainWindow;
        private MessageConverterWindow _messageConverterWindow;
        private IEventAggregator _eventAggregator;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _bootstrapper = new Bootstrapper();
            _container = _bootstrapper.Bootstrap();

            _eventAggregator = _container.Resolve<IEventAggregator>();

            _mainWindow = _container.Resolve<MainWindow>();
            _messageConverterWindow = _container.Resolve<MessageConverterWindow>();

            _eventAggregator.GetEvent<OpenMessageConverterWindowEvent>().Subscribe(OnOpenMessageConverterWindow);
            _eventAggregator.GetEvent<OpenMainWindowEvent>().Subscribe(OnOpenMainWindow);

            _mainWindow.Show();
        }

        private void OnOpenMainWindow()
        {
            _messageConverterWindow.Hide();
            _mainWindow.Show();
        }

        private void OnOpenMessageConverterWindow()
        {
            _mainWindow.Hide();
            _messageConverterWindow.Show();
        }
    }
}
