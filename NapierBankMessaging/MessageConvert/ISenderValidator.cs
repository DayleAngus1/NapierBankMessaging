using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.MessageConvert
{ 
    public interface ISenderValidator
    {
        public bool ValidateSender(string sender);
    }
}
