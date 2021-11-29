using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.MessageConvert
{
    public class InvalidSenderException : Exception
    {
        public InvalidSenderException(string errorMessage) : base(errorMessage)
        {
        }
    }
}
