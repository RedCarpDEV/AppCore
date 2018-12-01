using System;
using System.Collections.Generic;

namespace RCd.AppCore.Logging
{

    public class Logger : ILogger, ILoggingConfiguration
    {

        #region Constructors

        public Logger()
        {
            _handlers = new List<Action<LogLevel, DateTime, string>>();

            _actualLevel = LogLevel.Debug;
        }

        #endregion

        #region Member

        private readonly List<Action<LogLevel, DateTime, string>> _handlers;

        private LogLevel? _actualLevel;

        #endregion

        #region Methods

        public void Configure(LogLevel? actualLevel)
        {
            _actualLevel = actualLevel;
        }

        public void RegisterHandler(Action<LogLevel, DateTime, string> handler)
        {
            if (handler == null) return;

            _handlers.Add(handler);
        }

        public void Log(LogLevel level, string message)
        {
            if (_actualLevel == null) return;
            if (level < _actualLevel) return;

            var logDate = DateTime.Now;

            _handlers.ForEach(h => h.Invoke(level, logDate, message ?? string.Empty));
        }

        public void Dispose()
        {
            _handlers.Clear();
        }

        #endregion

    }

}