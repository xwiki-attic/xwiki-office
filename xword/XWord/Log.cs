using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;

namespace XWriter
{
    /// <summary>
    /// A class for writing logs to IsolatedStorage
    /// </summary>
    public class Log
    {
        private static string eventSource = "XOffice";
        private static string logName = "XWord";

        /// <summary>
        /// Writes a entry to the application's log.
        /// </summary>
        /// <param name="message">The logged message.</param>
        /// <param name="type">The type of the message.</param>
        public static void Write(String message, EventLogEntryType type)
        {
            if (EventLog.Exists(logName))
            {
                EventLog.CreateEventSource(eventSource, logName);
            }
            else
            {
                EventLog.WriteEntry(eventSource, message, type);
            }
        }

        /// <summary>
        /// Reports an error.
        /// </summary>
        /// <param name="message">The log message.</param>
        public static void Error(String message)
        {
            Write(message, EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes some information to the log.
        /// </summary>
        /// <param name="message">The log message.</param>
        public static void Information(String message)
        {
            Write(message, EventLogEntryType.Information);
        }

        /// <summary>
        /// Reports an success of an audit/assertion.
        /// </summary>
        /// <param name="message">The log message.</param>
        public static void Success(String message)
        {
            Write(message, EventLogEntryType.SuccessAudit);
        }

        /// <summary>
        /// Writes an warning message to the logs.
        /// </summary>
        /// <param name="message">The log message.</param>
        public static void Warning(String message)
        {
            Write(message, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Reports a failed assertion.
        /// </summary>
        /// <param name="message">The log message.</param>
        public static void FailAssert(String message)
        {
            Write(message, EventLogEntryType.FailureAudit);
        }

        /// <summary>
        /// Logs a summary info about an exception.
        /// </summary>
        /// <param name="ex">The logged exception.</param>
        public static void ExceptionSummary(Exception ex)
        {
            String message = "Source:   " + ex.Source + Environment.NewLine;
            message += "Type:     " + ex.GetType().ToString() + Environment.NewLine;
            message += "Message:  " + ex.Message + Environment.NewLine;
            message += "Help link " + ex.HelpLink;
            Write(message, EventLogEntryType.Error);
        }

        /// <summary>
        /// Logs info about an exception.
        /// </summary>
        /// <param name="ex">The logged exception.</param>
        public static void Exception(Exception ex)
        {
            String message = "Source:   " + ex.Source + Environment.NewLine;
            message += "Type:     " + ex.GetType().ToString() + Environment.NewLine;
            message += "Message:  " + ex.Message + Environment.NewLine;
            message += "Help link " + ex.HelpLink + Environment.NewLine;
            string exceptionData = "Exception data:" + Environment.NewLine;
            foreach (Object key in ex.Data.Keys)
            {
                exceptionData += "        " + key.ToString() + " -> " + ex.Data[key].ToString();
            }
            message += exceptionData + Environment.NewLine;
            message += Environment.NewLine + "Stack trace:" + Environment.NewLine + ex.StackTrace;
            Write(message, EventLogEntryType.Error);
        }
    }
}
