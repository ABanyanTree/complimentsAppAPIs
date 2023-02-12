using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.NLogger
{
    public class ErrorLoggerNLog :ILog
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public ErrorLoggerNLog()
        {

        }

        public void Information(string message)
        {
            logger.Info(message);
        }

        public void Warning(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }
    }
}
