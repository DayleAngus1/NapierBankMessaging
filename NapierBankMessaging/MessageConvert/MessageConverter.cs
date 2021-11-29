using System;
using System.Collections.Generic;
using System.Text;
using NapierBankMessaging.Model;

namespace NapierBankMessaging.MessageConvert
{
    public abstract class MessageConverter : ISenderValidator    
    {
        public abstract bool ConvertMessage(string header, string body, Message message);
        public abstract bool ValidateSender(string sender);
    }
}
