using System;

namespace RCd.AppCore.Logging
{

    public interface ILoggingConfiguration : IDisposable
    {

        #region Methods

        void Configure(LogLevel? actualLevel);

        void RegisterHandler(Action<LogLevel, DateTime, string> handler);

        #endregion

    }

}