#region LGPL license
/*
 * See the NOTICE file distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * This software is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this software; if not, write to the Free
 * Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
 * 02110-1301 USA, or see the FSF site: http://www.fsf.org.
 */
#endregion //license

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using XWiki.Logging;

namespace XWiki
{
    /// <summary>
    /// A class for writing logs to IsolatedStorage
    /// </summary>
    public class Log
    {
        private static string eventSource = "XWiki";
        private static string logName = "XOffice";
        private static LogMode mode = LogMode.WindowsLog;
        private static bool fileCreated = false;

        private static bool cantLog = false;

        //The message for EventLog can not exceed 32766 bytes
        private const int MAX_MSG_SIZE = 32766;

        /// <summary>
        /// Writes a entry to the appWindows logging system.
        /// </summary>
        /// <param name="message">The logged message.</param>
        /// <param name="type">The type of the message.</param>
        public static void WriteWindowsLog(String message, EventLogEntryType type)
        {
            try
            {
                String msg = (message.Length > MAX_MSG_SIZE) ? message.Substring(0, MAX_MSG_SIZE) : message;
                if (!EventLog.Exists(logName))
                    EventLog.CreateEventSource(eventSource, logName);
                EventLog.WriteEntry(eventSource, msg, type);
            }
            catch (System.Security.SecurityException e)
            {
                // can't CreateEventSource() in Vista/Win7 without permissions,
                // it has to be done in the installer with Admin privileges
                mode = LogMode.FileLog;
                WriteFileLog(message, type);
            }
            catch (InvalidOperationException ex)
            {
                mode = LogMode.FileLog;
                WriteFileLog(message, type);
            }
        }

        public static void WriteFileLog(String message, EventLogEntryType type)
        {
            // no need to check for this in WriteWindowsLog(), as this is our fallback
            if (cantLog)
                return;

            string logFileName = System.IO.Path.GetTempPath() + "xoffice.log";
            FileInfo logFile = new FileInfo(logFileName);
            try
            {
                StreamWriter writer = null;
                if (!fileCreated)
                {
                    if (logFile.Exists)
                    {
                        writer = logFile.AppendText();
                    }
                    else
                    {
                        writer = logFile.CreateText();
                    }
                }
                writer.WriteLine(type.ToString() + ": " + message);
                writer.Close();
            }
            catch (Exception e)
            {
                cantLog = true;
                UserNotifier.Error("Unable to log to the Systems log or" + Environment.NewLine +
                    "to my fallback option: " + logFile.FullName + Environment.NewLine +
                    "Please try reinstalling XOffice");
            }
        }

        public static void Write(String message, EventLogEntryType type)
        {
            if (mode == LogMode.WindowsLog)
            {
                WriteWindowsLog(message, type);
            }
            else if (mode == LogMode.FileLog)
            {
                WriteFileLog(message, type);
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
