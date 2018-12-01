using System;
using System.Threading.Tasks;

namespace RCd.AppCore.Messaging
{

    public interface IMessageChannel : IDisposable
    {

        #region Methods

        Task PublishAsync<TMessage>(TMessage message);

        ISubscriber<TMessage> Subscribe<TMessage>();

        bool IsSubscribed(ISubscriber subscriber);

        bool Unsubscribe(ISubscriber subscriber);

        #endregion

    }

}