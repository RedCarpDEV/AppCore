using System;

namespace RCd.AppCore.Messaging
{

    public interface IMessageBus : IDisposable
    {

        #region Methods

        IMessageBus RegisterChannel(string channelId);

        bool IsChannelRegistered(string channelId);

        IMessageChannel GetChannel(string channelId);

        #endregion

    }

}