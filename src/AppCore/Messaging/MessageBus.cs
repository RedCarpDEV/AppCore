using System;
using System.Collections.Generic;

namespace RCd.AppCore.Messaging
{

    public class MessageBus : IMessageBus
    {

        #region Constructors

        public MessageBus(Func<string, IMessageChannel> messageChannelFactory)
        {
            _messageChannelFactory = messageChannelFactory ?? throw new ArgumentNullException(nameof(messageChannelFactory));
            _channels = new Dictionary<string, IMessageChannel>();
        }

        #endregion

        #region Member

        private readonly Func<string, IMessageChannel> _messageChannelFactory;

        private readonly Dictionary<string, IMessageChannel> _channels;

        #endregion

        #region Methods

        public IMessageBus RegisterChannel(string channelId)
        {
            if (string.IsNullOrEmpty(channelId)) throw new ArgumentNullException(nameof(channelId));
            if (IsChannelRegistered(channelId)) throw new InvalidOperationException($"Channel registered already: {channelId}");

            _channels[channelId] = _messageChannelFactory.Invoke(channelId);

            return this;
        }

        public bool IsChannelRegistered(string channelId)
        {
            if (string.IsNullOrEmpty(channelId)) return false;
            return _channels.ContainsKey(channelId);
        }

        public IMessageChannel GetChannel(string channelId)
        {
            if (string.IsNullOrEmpty(channelId)) throw new KeyNotFoundException($"Channel not found: {channelId}");
            return _channels[channelId];
        }

        public void Dispose()
        {
            foreach (var channel in _channels.Values)
            {
                channel.Dispose();
            }
            _channels.Clear();
        }

        #endregion

    }

}