using System;
using System.Collections.Generic;
using System.Text;
using NapierBankMessaging.Import;

namespace NapierBankMessaging.MessageConvert
{
    public class MessageConverterDictionaryBuilder : IMessageConverterDictionaryBuilder
    {
        private readonly ITextSpeakConverter _converter;
        private readonly IIncidentListImporter _incidentListImporter;

        public MessageConverterDictionaryBuilder(ITextSpeakConverter converter, IIncidentListImporter incidentListImporter)
        {
            _converter = converter;
            _incidentListImporter = incidentListImporter;
        }
        public Dictionary<string, MessageConverter> Build()
        {
            return new Dictionary<string, MessageConverter>(StringComparer.InvariantCultureIgnoreCase)
            {
                {"S", new SmsMessageConverter(_converter)},
                {"E", new EmailMessageConverter(_incidentListImporter)},
                {"T", new TweetMessageConverter(_converter)}
            };

        }
    }
}
