using System;

namespace CopperMatchmaking.Info
{
    public static class Log
    {
        public static void Info(object message) => CopperLogger.UninitializedLogInfo(message);
        public static void Warning(object message) => CopperLogger.UninitializedLogWarning(message);
        public static void Error(object message) => CopperLogger.UninitializedLogError(message);
        public static void Error(Exception e) => Error($"[{e.GetType()}] {e.Message} \n {e.StackTrace}");

        public static void Assert(bool condition, object message)
        {
            if (!condition)
                Error(message);
        }
    }

    internal static partial class CopperLogger
    {
        public delegate void BaseLog(object message);

        public static BaseLog? Info;
        public static BaseLog? Warning;
        public static BaseLog? Error;

        private static bool initialized = false;

        private static bool includeTimestamps = false;


        public static void Initialize(bool timestamps = true)
        {
            Initialize(LogInfo, LogWarning, LogError, includeTimestamps);
        }

        public static void Initialize(BaseLog infoLog, BaseLog warningLog, BaseLog errorLog, bool timestamps = true)
        {
            if (initialized)
                return;
            initialized = true;

            Info = infoLog;
            Warning = warningLog;
            Error = errorLog;

            includeTimestamps = timestamps;
        }


        internal static void UninitializedLogInfo(object message)
        {
            Initialize(LogInfo, LogWarning, LogError);
            LogInfo(message);
        }

        internal static void UninitializedLogWarning(object message)
        {
            Initialize(LogInfo, LogWarning, LogError);
            LogWarning(message);
        }

        internal static void UninitializedLogError(object message)
        {
            Initialize(LogInfo, LogWarning, LogError);
            LogError(message);
        }

        public static void LogInfo(object message) => WriteBaseLog("INFO", message, ConsoleColor.DarkGray);
        public static void LogWarning(object message) => WriteBaseLog("WARN", message, ConsoleColor.DarkYellow);
        public static void LogError(object message) => WriteBaseLog("ERROR", message, ConsoleColor.DarkRed);

        private static void WriteBaseLog(string prefix, object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            var logString = "";

            if (includeTimestamps)
                // logString += $"[{DateTime.Now.ToShortTimeString()}] ";
                logString += $"[{DateTime.Now:HH:mm:ss}] ";
            logString += $"[{prefix}] {message}";

            Console.WriteLine(logString);
            Console.ResetColor();
        }
    }
}