using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NapierBankMessaging.Import
{
    public class IncidentListCsvImporter : IIncidentListImporter
    {
        private readonly string _filename;

        public IncidentListCsvImporter(string filename)
        {
            _filename = filename;
        }
        public List<string> ImportIncidentList()
        {
            var text = File.ReadAllText(_filename);
            return text.Split(',').ToList();
        }
    }
}