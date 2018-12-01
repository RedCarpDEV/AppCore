using System;
using System.Threading.Tasks;

namespace RCd.AppCore.Messaging
{

    public interface ISubscriber : IDisposable
    {

        #region Properties

        Type MessageType { get; }

        #endregion

    }

    public interface ISubscriber<TMessage> : ISubscriber
    {

        #region Methods

        void Filter(Func<TMessage, bool> messageFilter);

        void Run(Func<TMessage, Task> messageHandler);

        #endregion

    }

}