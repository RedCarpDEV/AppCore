using System;
using System.Threading.Tasks;

namespace RCd.AppCore.Messaging
{

    public class Subscriber<TMessage> : ISubscriber<TMessage>
    {

        #region Constructors

        public Subscriber(IMessageChannel messageChannel)
        {
            _messageChannel = messageChannel ?? throw new ArgumentNullException(nameof(messageChannel));
        }

        #endregion

        #region Member

        private readonly IMessageChannel _messageChannel;

        private Func<TMessage, bool> _messageFilter;

        private Func<TMessage, Task> _messageHandler;

        #endregion

        #region Properties

        public Type MessageType => typeof(TMessage);

        #endregion

        #region Methods

        void ISubscriber<TMessage>.Filter(Func<TMessage, bool> messageFilter)
        {
            _messageFilter = messageFilter;
        }

        void ISubscriber<TMessage>.Run(Func<TMessage, Task> messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public bool Filter(TMessage message)
        {
            return _messageFilter?.Invoke(message) ?? true;
        }

        public Task RunAsync(TMessage message)
        {
            return _messageHandler?.Invoke(message) ?? Task.CompletedTask;
        }

        public void Dispose()
        {
            _messageChannel.Unsubscribe(this);
        }

        #endregion

    }

}