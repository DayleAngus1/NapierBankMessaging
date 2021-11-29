using NapierBankMessaging.Model;

namespace NapierBankMessaging.ViewModel
{
    public interface IMessageFactory
    {
        public Message CreateMessage();
    }
}