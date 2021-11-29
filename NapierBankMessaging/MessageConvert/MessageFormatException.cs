using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.MessageConvert
{
    public class MessageFormatException : Exception
    {
        public MessageFormatException(string errorMessage) : base(errorMessage)
        {
            
        }
    }
}
