using System;
using System.Threading.Tasks;

namespace RCd.AppCore.Messaging
{

    public static class MessageChannelExtensions
    {

        #region Methods


        public static ISubscriber<TMessage> Subscribe<TMessage>(this IMessageChannel messageChannel, Func<TMessage, Task> messageHandler)
        {
            var subscriber = messageChannel.Subscribe<TMessage>();
            subscriber.Run(messageHandler);
            return subscriber;
        }

        #endregion

    }

}