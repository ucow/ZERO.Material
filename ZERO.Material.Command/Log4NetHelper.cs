using System;
using System.IO;
using System.Reflection;
using log4net;

namespace ZERO.Material.Command
{
    public class Log4NetHelper
    {
        private static readonly log4net.ILog LogInfo;
        private static readonly log4net.ILog LogError;
        private static readonly log4net.ILog LogMonitor;

        static Log4NetHelper()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "/Web.config"));
            LogInfo = log4net.LogManager.GetLogger("loginfo");
            LogError = log4net.LogManager.GetLogger("logerror");
            LogMonitor = log4net.LogManager.GetLogger("logmonitor");
        }

        public static void Error(string ErrorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                LogError.Error(ErrorMsg, ex);
            }
            else
            {
                LogError.Error(ErrorMsg);
            }
        }

        public static void Info(string Msg)
        {
            LogInfo.Info(Msg);
        }

        public static void Monitor(string Msg)
        {
            LogMonitor.Info(Msg);
        }
    }
}