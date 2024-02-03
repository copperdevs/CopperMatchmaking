using System;

namespace CopperMatchmaking.Info
{
    public static class Log
    {
        public static void Info(object message) => CopperLogger.LogInfo(message);
        public static void Warning(object message) => CopperLogger.LogWarning(message);
        public static void Error(object message) => CopperLogger.LogError(message);
        public static void Error(Exception e) => Error($"[{e.GetType()}] {e.Message} \n {e.StackTrace}");

        public static void Assert(bool condition, object message)
        {
            if (!condition)
                Error(message);
        }
    }

    public static class CopperLogger
    {
        public delegate void BaseLog(object message);

        private static BaseLog? info;
        private static BaseLog? warning;
        private static BaseLog? error;

        private static bool initialized = false;

        private static bool includeTimestamps = false;


        public static void Initialize(bool timestamps = true)
        {
            Initialize(InternalLogInfo, InternalLogWarning, InternalLogError, includeTimestamps);
        }

        public static void Initialize(BaseLog infoLog, BaseLog warningLog, BaseLog errorLog, bool timestamps = true)
        {
            if (initialized)
                return;
            initialized = true;

            info = infoLog;
            warning = warningLog;
            error = errorLog;

            includeTimestamps = timestamps;
        }
        
        public static void LogInfo(object message) => info?.Invoke(message);

        public static void LogWarning(object message) => warning?.Invoke(message);

        public static void LogError(object message) => error?.Invoke(message);
        
        public static void InternalLogInfo(object message) => WriteBaseLog("INFO", message, ConsoleColor.DarkGray);
        public static void InternalLogWarning(object message) => WriteBaseLog("WARN", message, ConsoleColor.DarkYellow);
        public static void InternalLogError(object message) => WriteBaseLog("ERROR", message, ConsoleColor.DarkRed);

        private static void WriteBaseLog(string prefix, object message, ConsoleColor color)
        {
            Console.ForegroundColor = color;

            var logString = "";

            if (includeTimestamps)
                logString += $"[{DateTime.Now:HH:mm:ss}] ";
            logString += $"[{prefix}] {message}";

            Console.WriteLine(logString);
            Console.ResetColor();
        }
    }
}