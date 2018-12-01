using RCd.AppCore.Logging;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace RCd.AppCore.Test
{

    public class LoggerTest
    {

        [Fact]
        public void Register_Does_Nothing_If_Handler_IsNull()
        {
            var logger = new Logger();
            logger.RegisterHandler(null);
        }

        [Fact]
        public void Log_Runs_All_Registered_Handler_Correctly()
        {
            const string logMessage = nameof(logMessage);
            var logLevel = LogLevel.Debug;

            var message1 = string.Empty;
            var message2 = string.Empty;

            void OnLog1(LogLevel level, DateTime logDate, string message)
            {
                message1 = message;
            }

            void OnLog2(LogLevel level, DateTime logDate, string message)
            {
                message2 = message;
            }

            var logger = new Logger();
            logger.Configure(logLevel);
            logger.RegisterHandler(OnLog1);
            logger.RegisterHandler(OnLog2);

            logger.Log(logLevel, logMessage);

            Assert.Equal(logMessage, message1);
            Assert.Equal(logMessage, message2);
        }

        [Fact]
        public void Log_Sets_LogDate()
        {
            const string logMessage = nameof(logMessage);
            var logLevel = LogLevel.Debug;

            DateTime? logDate1 = null;
            DateTime? logDate2 = null;

            void OnLog1(LogLevel level, DateTime logDate, string message)
            {
                logDate1 = logDate;
                Thread.Sleep(1);
            }

            void OnLog2(LogLevel level, DateTime logDate, string message)
            {
                logDate2 = logDate;
                Thread.Sleep(1);
            }

            var logger = new Logger();
            logger.Configure(logLevel);
            logger.RegisterHandler(OnLog1);
            logger.RegisterHandler(OnLog2);

            logger.Log(logLevel, logMessage);

            Assert.NotNull(logDate1);
            Assert.NotNull(logDate2);
            Assert.Equal(logDate1.Value.ToFileTimeUtc(), logDate2.Value.ToFileTimeUtc());
        }

        [Fact]
        public void Log_Only_Logs_ActualLevel()
        {
            var logged = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToDictionary(l => l, l => false);

            void OnLog(LogLevel logLevel, DateTime logDate, string message)
            {
                logged[logLevel] = true;
            }

            var logger = new Logger();
            logger.RegisterHandler(OnLog);

            foreach (var actualLevel in logged.Keys.ToList())
            {
                logger.Configure(actualLevel);

                foreach (var level in logged.Keys.ToList())
                {
                    logged[level] = false;

                    logger.Log(level, level.ToString());

                    if (level >= actualLevel)
                    {
                        Assert.True(logged[level]);
                    }
                    else
                    {
                        Assert.False(logged[level]);
                    }
                }
            }
        }

        [Fact]
        public void Log_Does_Not_Log_If_Logging_Is_Disabled()
        {
            var logged = false;

            void OnLog(LogLevel logLevel, DateTime logDate, string message)
            {
                logged = true;
            }

            var logger = new Logger();
            logger.Configure(null);
            logger.RegisterHandler(OnLog);

            var allLogLevels = Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>();

            foreach (var level in allLogLevels)
            {
                logger.Log(level, level.ToString());
            }

            Assert.False(logged);
        }

        [Fact]
        public void Log_Sets_Message_To_StringEmpty_If_Incoming_Message_Is_Null()
        {
            const string logMessage = nameof(logMessage);
            var logLevel = LogLevel.Debug;

            string exceptedMessage = null;

            void OnLog(LogLevel LogLevel, DateTime logDate, string message)
            {
                exceptedMessage = message;
            }

            var logger = new Logger();
            logger.Configure(logLevel);
            logger.RegisterHandler(OnLog);

            logger.Log(logLevel, null);

            Assert.Equal(string.Empty, exceptedMessage);
        }

    }

}