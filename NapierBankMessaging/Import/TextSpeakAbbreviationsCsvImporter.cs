using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NapierBankMessaging.Import
{
    public class TextSpeakAbbreviationsCsvImporter : ITextSpeakAbbreviationsImporter
    {
        private readonly string _filename;

        public TextSpeakAbbreviationsCsvImporter(string filename)
        {
            _filename = filename;
        }
        public Dictionary<string, string> ImportTextSpeakAbbreviations()
        {
            return File.ReadLines(_filename).Select(line => line.Split(','))
                .ToDictionary(line => line[0], line => line[1]);
        }
    }
}
