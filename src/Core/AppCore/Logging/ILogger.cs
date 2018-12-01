namespace RCd.AppCore.Logging
{

    public interface ILogger
    {

        #region Methods

        void Log(LogLevel level, string message);

        #endregion

    }

}