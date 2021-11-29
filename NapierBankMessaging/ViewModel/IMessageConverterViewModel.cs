using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NapierBankMessaging.ViewModel
{
    public interface IMessageConverterViewModel
    {
         public ICommand OpenMainWindowCommand { get; set; }
    }
}
