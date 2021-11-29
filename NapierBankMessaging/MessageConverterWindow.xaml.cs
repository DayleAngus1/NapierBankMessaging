using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NapierBankMessaging.ViewModel;

namespace NapierBankMessaging
{
    /// <summary>
    /// Interaction logic for MessageConverterWindow.xaml
    /// </summary>
    public partial class MessageConverterWindow : Window
    {
        private readonly IMessageConverterViewModel _viewModel;

        public MessageConverterWindow(IMessageConverterViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;

            DataContext = viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
    }
}
