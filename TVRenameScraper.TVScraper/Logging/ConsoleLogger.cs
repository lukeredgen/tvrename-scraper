using System;
using log4net;

namespace TVRenameScraper.TvScraper.Logging
{
    public static class ConsoleLogger
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(Program));

        public static void Log(string message)
        {
            Console.WriteLine(message);
            _log.Debug(message);
        }

        public static void LogStart(string message)
        {
            Console.Write(message);
            _log.Debug(message);
        }

        public static void LogEnd(string message)
        {
            Console.WriteLine(message);
            _log.Debug(message);
        }

        public static void Highlight(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            _log.Debug(message);
        }

        public static void Warning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
            _log.Warn(message);
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            _log.Error(message);
        }
    }
}
