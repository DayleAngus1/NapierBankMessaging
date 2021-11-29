using System;
using System.Collections.Generic;
using System.Text;

namespace NapierBankMessaging.Model
{
    public abstract class Message
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public string Sender { get; set; }
    }
}
