using System;

namespace NapierBankMessaging.MessageConvert
{
    public class MessageTypeMismatchException : Exception
    {
        public MessageTypeMismatchException(string errorMessage) : base(errorMessage)
        {
        }
    }
}