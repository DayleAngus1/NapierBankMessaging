using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.MessageConvert
{ 
    public interface IMessageConverterDictionaryBuilder
    {
        public Dictionary<string, MessageConverter> Build();
    }
}
