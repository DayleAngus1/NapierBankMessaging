using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NapierBankMessaging.Import;

namespace NapierBankMessaging.MessageConvert
{ 
    public class TextSpeakConverter : ITextSpeakConverter
    {
        private readonly Dictionary<string, string> _abbreviationsDictionary;

        public TextSpeakConverter(ITextSpeakAbbreviationsImporter importer)
        {
            _abbreviationsDictionary = importer.ImportTextSpeakAbbreviations();
        }

        public string ReplaceTextSpeakAbbreviations(string message)
        {
            return _abbreviationsDictionary.Keys.Aggregate(message, (current, key) => current.Replace(key, key + " <" + _abbreviationsDictionary[key] + ">"));
        }
    }
}
