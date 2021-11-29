using NapierBankMessaging.Model;

namespace NapierBankMessaging.ViewModel
{
    public class SmsMessageFactory : IMessageFactory
    {
        public Message CreateMessage()
        {
            return new SmsMessage();
        }
    }
}