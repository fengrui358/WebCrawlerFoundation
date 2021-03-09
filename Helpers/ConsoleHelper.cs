using System;
using Exceptionless;
using NLog;

namespace WebCrawlerFoundation.Helpers
{
    public static class ConsoleHelper
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Console(string msg, LogLevel logLevel = null)
        {
            msg = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}:{msg}";

            if (logLevel == null)
            {
                logLevel = LogLevel.Info;
            }

            Logger.Log(logLevel, msg);

            var defaultColor = System.Console.ForegroundColor;

            if (logLevel == LogLevel.Error || logLevel == LogLevel.Fatal)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
            }

            if (logLevel == LogLevel.Info)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
            }

            if (logLevel == LogLevel.Warn)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
            }

            System.Console.WriteLine(msg);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void Console(Exception exception)
        {
            var defaultColor = System.Console.ForegroundColor;

            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(exception);

            Logger.Error(exception);

            System.Console.ForegroundColor = defaultColor;

            exception.ToExceptionless().Submit();
        }
    }
}
