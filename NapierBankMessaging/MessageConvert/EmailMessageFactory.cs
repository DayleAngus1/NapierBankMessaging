using NapierBankMessaging.Model;

namespace NapierBankMessaging.ViewModel
{
    public class EmailMessageFactory : IMessageFactory
    {
        public Message CreateMessage() => new EmailMessage();
    }
}