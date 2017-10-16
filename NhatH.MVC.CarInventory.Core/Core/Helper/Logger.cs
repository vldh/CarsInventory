using log4net;
using System;

namespace NhatH.MVC.CarInventory.Core.Core.Helper
{
    public static class Logger
    {
        private static ILog _log;

        static Logger()
        {
            _log = log4net.LogManager.GetLogger(typeof(Logger));
            log4net.Config.XmlConfigurator.Configure();
        }

        public static void Info(string msg)
        {
            _log.Info(msg);
        }

        public static void Info(params string[] msg)
        {
            _log.Info(string.Join("", msg));
        }

        public static void InfoFormat(string format, params object[] args)
        {
            _log.InfoFormat(format, args);
        }

        public static void Debug(string message)
        {
            if (_log.IsDebugEnabled)
            {
                _log.Debug(message);
            }
        }

        public static void DebugFormat(string format, params object[] args)
        {
            _log.DebugFormat(format, args);
        }

        public static void Error(this Exception ex, string msg)
        {
            _log.Error(msg, ex);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            _log.ErrorFormat(format, args);
        }
    }
}
