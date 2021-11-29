using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.Import
{
    public interface ITextSpeakAbbreviationsImporter
    {
        public Dictionary<string, string> ImportTextSpeakAbbreviations();
    }
}
