using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.MessageConvert
{
    public interface IHeaderValidator
    {
        public bool ValidateHeader(string header);
    }
}
