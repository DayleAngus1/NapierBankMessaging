using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NapierBankMessaging.Model;

namespace NapierBankMessaging.MessageConvert
{
    public class SmsMessageConverter : MessageConverter    {
        private readonly ITextSpeakConverter _textSpeakConverter;

        public SmsMessageConverter(ITextSpeakConverter textSpeakConverter)
        {
            _textSpeakConverter = textSpeakConverter;
        }

        public override bool ConvertMessage(string header, string body, Message message)
        {
            if (!(message is SmsMessage))
            {
                throw new MessageTypeMismatchException("Message must be of type SmsMessage for this Converter");
            }
            using  var reader = new StringReader(body);

            var sender = reader.ReadLine();

            if (!ValidateSender(sender))
                throw new InvalidSenderException(
                    "Invalid Sender : The first line of an SMS body must be the Sender's an international phone number.");

            var messageText = reader.ReadToEnd();

            if (messageText != null && messageText.Length > 140)
                throw new MessageFormatException(
                    "Invalid SMS Message : SMS messages must be a maximum of 140 characters long");

            message.Sender = sender;
            message.Header = header;
            message.Body = _textSpeakConverter.ReplaceTextSpeakAbbreviations(messageText);

            return true;
        }

        public override bool ValidateSender(string sender)
        {
            return Regex.IsMatch(sender, "^([+]|[00]{2})([0-9]|[ -])*");
        }
    }
}
