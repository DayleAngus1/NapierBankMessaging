using NapierBankMessaging.Model;

namespace NapierBankMessaging.ViewModel
{
    public class TweetMessageFactory : IMessageFactory
    {
        public Message CreateMessage()
        {
            return new TweetMessage();
        }
    }
}