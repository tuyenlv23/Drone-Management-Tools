using System;
using System.Collections.Generic;

namespace Drone_Management_Tools.Utilities
{
    public class LogUtils
    {
        private static NLog.Logger nlog = NLog.LogManager.GetCurrentClassLogger();
        private static Queue<string> eventLogs = new Queue<string>();
        private static Queue<LogContent> logContents = new Queue<LogContent>();
        private static object locker = new object();

        public static void ShowLogInfo(string message)
        {
            nlog.Info(message);
        }

        public static void ShowLogInfo(Exception ex, bool isStackTrace)
        {
            if (isStackTrace)
                nlog.Info("{0}\n{1}", ex.Message, ex.StackTrace);
            else
                nlog.Info(ex.Message);
        }

        public static void ShowLogWarn(string message)
        {
            nlog.Warn(message);
        }

        public static void ShowLogWarn(Exception ex, bool isStackTrace)
        {
            if (isStackTrace)
                nlog.Warn("{0}\n{1}", ex.Message, ex.StackTrace);
            else
                nlog.Warn(ex.Message);
        }

        public static void ShowLogError(string message)
        {
            nlog.Error(message);
        }

        public static void ShowLogError(Exception ex, bool isStackTrace)
        {
            if (isStackTrace)
                nlog.Error("{0}\n{1}", ex.Message, ex.StackTrace);
            else
                nlog.Error(ex.Message);
        }

        public static void ShowLog(string message)
        {
            nlog.Debug(message);
        }

        public static void AddLog(string message)
        {
            lock (locker)
            {
                var eventLog = $"<{string.Format("{0:dd-MMM-yyyy HH:mm:ss.fff}", DateTime.Now)}> | {message}";
                eventLogs.Enqueue(eventLog);
            }
        }

        public static List<string> GetEventLogs()
        {
            lock (locker)
            {
                List<string> _logs = new List<string>();

                while (eventLogs.Count > 0)
                    _logs.Add(eventLogs.Dequeue());

                return _logs;
            }
        }

        public static void AddLog(string message, LogTypes logType)
        {
            lock (locker)
            {
                var logTime = $"<{string.Format("{0:dd-MMM-yyyy HH:mm:ss.fff}", DateTime.Now)}>";
                var eventLog = "";

                switch (logType)
                {
                    case LogTypes.Info:
                    case LogTypes.Info_Stack:
                        {
                            eventLog = $"{logTime} | INFO | {message}";
                            break;
                        }
                    case LogTypes.Warn:
                    case LogTypes.Warn_Stack:
                        {
                            eventLog = $"{logTime} | WARN | {message}";
                            break;
                        }
                    case LogTypes.Error:
                    case LogTypes.Error_Stack:
                        {
                            eventLog = $"{logTime} | ERROR | {message}";
                            break;
                        }
                }

                var logMessage = new LogContent(message, eventLog, logType);
                logContents.Enqueue(logMessage);
            }
        }

        public static List<LogContent> GetLogs()
        {
            lock (locker)
            {
                List<LogContent> _logs = new List<LogContent>();

                while (logContents.Count > 0)
                    _logs.Add(logContents.Dequeue());

                return _logs;
            }
        }
    }

    public class LogContent
    {
        public string Message { get; set; }
        public string EventLog { get; set; }
        public LogTypes LogType { get; set; }

        public LogContent(string message, string eventLog, LogTypes logType)
        {
            this.Message = message;
            this.EventLog = eventLog;
            this.LogType = logType;
        }
    }
}
