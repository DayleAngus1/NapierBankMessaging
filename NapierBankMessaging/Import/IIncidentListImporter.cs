using System.Collections.Generic;

namespace NapierBankMessaging.Import
{
    public interface IIncidentListImporter
    {
        public List<string> ImportIncidentList();
    }
}