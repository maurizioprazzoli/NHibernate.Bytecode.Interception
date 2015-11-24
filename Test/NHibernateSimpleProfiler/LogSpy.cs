﻿using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System.IO;

namespace NHibernateSimpleProfiler
{
    // A delegate type for hooking up change notifications.
    public delegate void SqlLogSpyStatisticsUpdated(SqlLogSpyStatistics sender, EventArgs e);

    public class LogSpy : IDisposable
    {
        public event SqlLogSpyStatisticsUpdated SqlLogSpyStatisticsUpdated;
        
        private readonly MemoryAppender appender;
        private readonly Logger logger;
        private readonly Level prevLogLevel;

        public LogSpy(ILog log, Level level)
        {
            logger = log.Logger as Logger;
            if (logger == null)
            {
                throw new Exception("Unable to get the logger");
            }

            // Change the log level to DEBUG and temporarily save the previous log level
            prevLogLevel = logger.Level;
            logger.Level = level;

            // Add a new MemoryAppender to the logger.
            appender = new MemoryAppender();
            logger.AddAppender(appender);

            // Clear the appender
            this.Appender.Clear();
        }

        public LogSpy(ILog log, bool disable)
            : this(log, disable ? Level.Off : Level.Debug)
        {
        }

        public LogSpy(ILog log) : this(log, false) { }
        public LogSpy(System.Type loggerType) : this(LogManager.GetLogger(loggerType), false) { }
        public LogSpy(System.Type loggerType, bool disable) : this(LogManager.GetLogger(loggerType), disable) { }

        public LogSpy(string loggerName) : this(LogManager.GetLogger(loggerName), false) { }
        public LogSpy(string loggerName, bool disable) : this(LogManager.GetLogger(loggerName), disable) { }

        public MemoryAppender Appender
        {
            get { return appender; }
        }

        public virtual string GetWholeLog()
        {
            var wholeMessage = new StringBuilder();
            foreach (LoggingEvent loggingEvent in Appender.GetEvents())
            {
                wholeMessage
                    .Append(loggingEvent.LoggerName)
                    .Append(" ")
                    .Append(loggingEvent.RenderedMessage)
                    .AppendLine();
            }
            return wholeMessage.ToString();
        }

        #region IDisposable Members

        public void Dispose()
        {
            // updateSqlLogSpyStatistics
            updateSqlLogSpyStatistics();
            // Restore the previous log level of the SQL logger and remove the MemoryAppender
            logger.Level = prevLogLevel;
            logger.RemoveAppender(appender);
        }

        #endregion

        private void updateSqlLogSpyStatistics()
        {
            SqlLogSpyStatistics sqlLogSpyStatistics;
            sqlLogSpyStatistics.NumberOfSQLSelectStatement = 0;
            sqlLogSpyStatistics.NumberOfSQLInsertStatement = 0;
            sqlLogSpyStatistics.NumberOfSQLUpdateStatement = 0;
            sqlLogSpyStatistics.NumberOfSQLDeleteStatement = 0;

            TextWriter wr = new StringWriter();

            // Update sqlLogSpyStatistics NumberOfSQLSelectStatement
            foreach (var sqlEvent in this.Appender.GetEvents())
            {
                sqlEvent.WriteRenderedMessage(wr);
                if (wr.ToString().ToUpper().TrimStart().Contains("SELECT"))
                {
                    sqlLogSpyStatistics.NumberOfSQLSelectStatement++;
                }
            }
            // Update sqlLogSpyStatistics NumberOfSQLInsertStatement
            foreach (var sqlEvent in this.Appender.GetEvents())
            {
                sqlEvent.WriteRenderedMessage(wr);
                if (wr.ToString().ToUpper().TrimStart().Contains("INSERT INTO"))
                {
                    sqlLogSpyStatistics.NumberOfSQLInsertStatement++;
                }
            }
            // Update sqlLogSpyStatistics NumberOfSQLUpdateStatement
            foreach (var sqlEvent in this.Appender.GetEvents())
            {
                sqlEvent.WriteRenderedMessage(wr);
                if (wr.ToString().ToUpper().TrimStart().Contains("UPDATE"))
                {
                    sqlLogSpyStatistics.NumberOfSQLUpdateStatement++;
                }
            }
            // Update sqlLogSpyStatistics NumberOfSQLDeleteStatement
            foreach (var sqlEvent in this.Appender.GetEvents())
            {
                sqlEvent.WriteRenderedMessage(wr);
                if (wr.ToString().ToUpper().TrimStart().Contains("DELETE"))
                {
                    sqlLogSpyStatistics.NumberOfSQLDeleteStatement++;
                }
            }

            if (SqlLogSpyStatisticsUpdated != null)
            {
                SqlLogSpyStatisticsUpdated(sqlLogSpyStatistics, EventArgs.Empty);
            }
        }
    }
}
