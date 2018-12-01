using System;

namespace RCd.AppCore.Logging
{

    public static class LoggingExtensions
    {

        #region Methods

        public static void Debug(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Debug, message);
        }

        public static void Info(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Info, message);
        }

        public static void Warn(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Warn, message);
        }

        public static void Error(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Error, message);
        }

        public static void Error(this ILogger logger, Exception exception)
        {
            logger.Log(LogLevel.Error, exception.ToString());
        }

        public static void UseDebugging(this ILoggingConfiguration loggingConfiguration)
        {
            void Log(LogLevel logLevel, DateTime logDate, string logMessage)
            {
                System.Diagnostics.Debug.WriteLine($"{logDate:yyyy-MM-dd HH:mm:ss.fff}\n{logMessage}", logLevel.ToString());
            }

            loggingConfiguration.RegisterHandler(Log);
        }

        #endregion

    }

}