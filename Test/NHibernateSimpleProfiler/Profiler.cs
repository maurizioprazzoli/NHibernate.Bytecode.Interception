using NHibernate;
using NHibernate.Stat;
using System;
using System.Collections.Generic;

namespace NHibernateSimpleProfiler
{
    public static class Profiler
    {
        private static ISessionFactory sessionFactory;
        private static Stack<Statistics> snapshotStack;
        private static SqlLogSpyStatistics sqlLogSpyStatistics;

        public static ISessionFactory SetSessionFactory
        {
            set
            {
                if (value == null || !value.Statistics.IsStatisticsEnabled)
                {
                    throw new NHibernateSimpleProfilerException("NHibernate session factory has not statistics enabled, please enable befor call this. See generate_statistics");
                }

                sessionFactory = value;
                snapshotStack = new Stack<Statistics>();
            }
        }

        public static void TakeSnapshot()
        {
            if (sessionFactory == null || !sessionFactory.Statistics.IsStatisticsEnabled)
            {
                throw new NHibernateSimpleProfilerException("NHibernate session factory has not statistics enabled, please anable befor call this. See generate_statistics");
            }

            Statistics currentStatisticsSnapshot = (Statistics)(StatisticsImpl)sessionFactory.Statistics;
            currentStatisticsSnapshot.UpdateSqlLogSpyStatistics(sqlLogSpyStatistics);
            snapshotStack.Push(currentStatisticsSnapshot);
        }

        public static Statistics GetDifferenceFromLastSnapshot()
        {
            if (snapshotStack.Count < 1)
            {
                throw new NHibernateSimpleProfilerException("No Snapshot found!");
            }
            Statistics currentStatisticsSnapshot = (Statistics)(StatisticsImpl)sessionFactory.Statistics;
            currentStatisticsSnapshot.UpdateSqlLogSpyStatistics(sqlLogSpyStatistics);
            return currentStatisticsSnapshot - snapshotStack.Peek();
        }

        public static Statistics GetDifferenceFromLastSnapshotAndTakeSnapshot()
        {
            if (snapshotStack.Count < 1)
            {
                throw new NHibernateSimpleProfilerException("No Snapshot found!");
            }
            Statistics currentStatisticsSnapshot = (Statistics)(StatisticsImpl)sessionFactory.Statistics;
            currentStatisticsSnapshot.UpdateSqlLogSpyStatistics(sqlLogSpyStatistics);
            snapshotStack.Push(currentStatisticsSnapshot);
            return currentStatisticsSnapshot - snapshotStack.Peek();
        }

        public static SqlLogSpy LogSpy
        {
            get
            {
                var sqlLogSpy = new SqlLogSpy();
                sqlLogSpy.SqlLogSpyStatisticsUpdated += new SqlLogSpyStatisticsUpdated(updatedSqlLogSpyStatistics);
                return sqlLogSpy;
            }
        }

      // This will be called whenever the list changes.
        private static void updatedSqlLogSpyStatistics(SqlLogSpyStatistics updatedSqlLogSpyStatistics, EventArgs e) 
      {
          sqlLogSpyStatistics.NumberOfSQLSelectStatement += updatedSqlLogSpyStatistics.NumberOfSQLSelectStatement;
          sqlLogSpyStatistics.NumberOfSQLInsertStatement += updatedSqlLogSpyStatistics.NumberOfSQLInsertStatement;
          sqlLogSpyStatistics.NumberOfSQLUpdateStatement += updatedSqlLogSpyStatistics.NumberOfSQLUpdateStatement;
          sqlLogSpyStatistics.NumberOfSQLDeleteStatement += updatedSqlLogSpyStatistics.NumberOfSQLDeleteStatement;
      }

    }
}
