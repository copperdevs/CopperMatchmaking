using System;

namespace CopperMatchmaking.Info
{
    /// <summary>
    /// Log class 
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Logs an info message to the console
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Info(object message) => CopperLogger.LogInfo(message);

        /// <summary>
        /// Logs an warning message to the console
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Warning(object message) => CopperLogger.LogWarning(message);

        /// <summary>
        /// Logs an error message to the console
        /// </summary>
        /// <param name="message">Message to log</param>
        public static void Error(object message) => CopperLogger.LogError(message);

        /// <summary>
        /// Logs an exception to the console
        /// </summary>
        /// <param name="e">Error to log</param>
        public static void Error(Exception e) => Error($"[{e.GetType()}] {e.Message} \n {e.StackTrace}");

        /// <summary>
        /// Checks for a condition, and if false logs the message
        /// </summary>
        /// <param name="condition">Condition to check</param>
        /// <param name="message">Message to log</param>
        public static void Assert(bool condition, object message)
        {
            if (!condition)
                Error(message);
        }
    }

    /// <summary>
    /// Main logger class
    /// </summary>
    public static class CopperLogger
    {
        /// <summary>
        /// Log action
        /// </summary>
        public delegate void BaseLog(object message);

        private static BaseLog? info;
        private static BaseLog? warning;
        private static BaseLog? error;

        private static bool initialized;

        private static bool includeTimestamps;


        /// <summary>
        /// Initialize logger with internal log functions
        /// </summary>
        /// <param name="timestamps">Log messages with timestamp</param>
        public static void Initialize(bool timestamps = true)
        {
            Initialize(InternalLogInfo, InternalLogWarning, InternalLogError, includeTimestamps);
        }

        /// <summary>
        /// Initialize logger with custom log functions
        /// </summary>
        /// <param name="infoLog"></param>
        /// <param name="warningLog"></param>
        /// <param name="errorLog"></param>
        /// <param name="timestamps">Log messages with timestamp</param>
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

        /// <summary>
        /// Uses log action to log a message
        /// </summary>
        /// <param name="message"></param>
        public static void LogInfo(object message) => info?.Invoke(message);

        /// <summary>
        /// Uses warning log action to log a message
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(object message) => warning?.Invoke(message);

        /// <summary>
        /// Uses error log action to log a message
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(object message) => error?.Invoke(message);

        /// <summary>
        /// Base internal info log
        /// </summary>
        /// <param name="message"></param>
        public static void InternalLogInfo(object message) => WriteBaseLog("INFO", message, ConsoleColor.DarkGray);

        /// <summary>
        /// Base internal warning log
        /// </summary>
        /// <param name="message"></param>
        public static void InternalLogWarning(object message) => WriteBaseLog("WARN", message, ConsoleColor.DarkYellow);

        /// <summary>
        /// Base internal error
        /// </summary>
        /// <param name="message"></param>
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