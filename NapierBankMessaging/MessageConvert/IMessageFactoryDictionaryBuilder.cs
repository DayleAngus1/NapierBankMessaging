using System;
using System.Collections.Generic;

namespace NapierBankMessaging.ViewModel
{
    public interface IMessageFactoryDictionaryBuilder
    {
        public Dictionary<string, IMessageFactory> Build();
    }

    public class MessageFactoryDictionaryBuilder : IMessageFactoryDictionaryBuilder
    {
        public Dictionary<string, IMessageFactory> Build()
        {
            return new Dictionary<string, IMessageFactory>(StringComparer.CurrentCultureIgnoreCase)
            {
                {"S", new SmsMessageFactory()},
                {"E", new EmailMessageFactory()},
                {"T", new TweetMessageFactory()}
            };
        }
    }
}