using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NapierBankMessaging.MessageConvert
{
    public class HeaderValidator : IHeaderValidator
    {
        public bool ValidateHeader(string header)
        {
            var regex = new Regex(@"^[S,E,T,s,e,t]\d{9}\Z");

            return regex.IsMatch(header);
        }
    }
}
