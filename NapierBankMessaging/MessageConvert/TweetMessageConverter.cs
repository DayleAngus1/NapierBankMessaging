using System;
using System.Collections.Generic;
using System.Text;
using NapierBankMessaging.Model;

namespace NapierBankMessaging.MessageConvert
{
    public class TweetMessageConverter : MessageConverter
    {
        private readonly ITextSpeakConverter _converter;

        public TweetMessageConverter(ITextSpeakConverter converter)
        {
            _converter = converter;
        }

        public override bool ConvertMessage(string header, string body, Message message)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateSender(string sender)
        {
            throw new NotImplementedException();
        }
    }
}
