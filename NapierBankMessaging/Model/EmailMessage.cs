using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace NapierBankMessaging.Model
{
    public class EmailMessage : Message
    {
        public string Type { get; set; }
        public string Subject { get; set; }
        public string SortCode { get; set; }
        public string NatureOfIncident { get; set; }
        public List<string> Quarantined { get; set; }
    }
}
