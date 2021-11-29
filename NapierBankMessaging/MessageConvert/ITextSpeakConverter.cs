using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.MessageConvert
{
    public interface ITextSpeakConverter
    {
        public string ReplaceTextSpeakAbbreviations(string message);
    }
}
