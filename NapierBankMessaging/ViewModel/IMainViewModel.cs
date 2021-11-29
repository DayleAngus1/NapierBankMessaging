using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace NapierBankMessaging.ViewModel
{
    public interface IMainViewModel
    {
        public ICommand OpenMessageConverterWindowCommand { get; set; }
    }
}
