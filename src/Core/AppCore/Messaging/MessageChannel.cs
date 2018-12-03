using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RCd.AppCore.Messaging
{

    public class MessageChannel : IMessageChannel
    {

        #region Constructors

        public MessageChannel()
        {
            _subscribers = new Dictionary<Type, List<ISubscriber>>();
        }

        #endregion

        #region Member

        private readonly Dictionary<Type, List<ISubscriber>> _subscribers;

        #endregion

        #region Methods

        public Task PublishAsync<TMessage>(TMessage message)
        {
            var messageType = GetMessageType<TMessage>();

            if (!_subscribers.ContainsKey(messageType)) return Task.CompletedTask;

            var query = _subscribers[messageType]
                 .Cast<Subscriber<TMessage>>()
                 .Where(s => s.Filter(message))
                 .Select(s => s.RunAsync(message));

            return Task.WhenAll(query);
        }

        public ISubscriber<TMessage> Subscribe<TMessage>()
        {
            var messageType = GetMessageType<TMessage>();

            var subscriber = new Subscriber<TMessage>(this);

            if (!_subscribers.ContainsKey(messageType))
            {
                _subscribers[messageType] = new List<ISubscriber>();
            }

            _subscribers[messageType].Add(subscriber);

            return subscriber;
        }

        public bool IsSubscribed(ISubscriber subscriber)
        {
            if (subscriber?.MessageType == null) return false;
            if (!_subscribers.ContainsKey(subscriber.MessageType)) return false;

            return _subscribers[subscriber.MessageType].Contains(subscriber);
        }

        public bool Unsubscribe(ISubscriber subscriber)
        {
            if (subscriber?.MessageType == null) return false;
            if (!_subscribers.ContainsKey(subscriber.MessageType)) return false;

            return _subscribers[subscriber.MessageType].Remove(subscriber);
        }

        public void Dispose()
        {
            _subscribers.SelectMany(x => x.Value).ToList().ForEach(s => s.Dispose());
            _subscribers.Clear();
        }

        private Type GetMessageType<TMessage>()
        {
            return typeof(TMessage);
        }

        #endregion

    }

}