
using NHibernate.Stat;
namespace NHibernateSimpleProfiler
{

    public struct Statistics
    {
        public long SessionCloseCount;
        public long SessionOpenCount;

        public long CloseStatementCount;
        public long PrepareStatementCount;

        public long EntityDeleteCount;
        public long EntityFetchCount;
        public long EntityInsertCount;
        public long EntityLoadCount;
        public long EntityUpdateCount;

        public long CollectionFetchCount;
        public long CollectionLoadCount;
        public long CollectionRecreateCount;
        public long CollectionRemoveCount;
        public long CollectionUpdateCount;

        public long SecondLevelCacheHitCount;
        public long SecondLevelCacheMissCount;
        public long SecondLevelCachePutCount;

        public long? NumberOfSQLSelectStatement;
        public long? NumberOfSQLInsertStatement;
        public long? NumberOfSQLUpdateStatement;
        public long? NumberOfSQLDeleteStatement;

        public static Statistics operator -(Statistics left, Statistics right)
        {
            Statistics statistics = new Statistics();

            statistics.SessionCloseCount = left.SessionCloseCount - right.SessionCloseCount;
            statistics.SessionOpenCount = left.SessionOpenCount - right.SessionOpenCount;

            statistics.CloseStatementCount = left.CloseStatementCount - right.CloseStatementCount;
            statistics.PrepareStatementCount = left.PrepareStatementCount - right.PrepareStatementCount;

            statistics.EntityFetchCount = left.EntityFetchCount - right.EntityFetchCount;
            statistics.EntityDeleteCount = left.EntityDeleteCount - right.EntityDeleteCount;
            statistics.EntityInsertCount = left.EntityInsertCount - right.EntityInsertCount;
            statistics.EntityLoadCount = left.EntityLoadCount - right.EntityLoadCount;
            statistics.EntityUpdateCount = left.EntityUpdateCount - right.EntityUpdateCount;

            statistics.CollectionFetchCount = left.CollectionFetchCount - right.CollectionFetchCount;
            statistics.CollectionLoadCount = left.CollectionLoadCount - right.CollectionLoadCount;
            statistics.CollectionRecreateCount = left.CollectionRecreateCount - right.CollectionRecreateCount;
            statistics.CollectionRemoveCount = left.CollectionRemoveCount - right.CollectionRemoveCount;
            statistics.CollectionUpdateCount = left.CollectionUpdateCount - right.CollectionUpdateCount;

            statistics.SecondLevelCacheHitCount = left.SecondLevelCacheHitCount - right.SecondLevelCacheHitCount;
            statistics.SecondLevelCacheMissCount = left.SecondLevelCacheMissCount - right.SecondLevelCacheMissCount;
            statistics.SecondLevelCachePutCount = left.SecondLevelCachePutCount - right.SecondLevelCachePutCount;

            statistics.NumberOfSQLSelectStatement = left.NumberOfSQLSelectStatement - right.NumberOfSQLSelectStatement;
            statistics.NumberOfSQLInsertStatement = left.NumberOfSQLInsertStatement - right.NumberOfSQLInsertStatement;
            statistics.NumberOfSQLUpdateStatement = left.NumberOfSQLUpdateStatement - right.NumberOfSQLUpdateStatement;
            statistics.NumberOfSQLDeleteStatement = left.NumberOfSQLDeleteStatement - right.NumberOfSQLDeleteStatement;

            return statistics;
        }

        public static explicit operator Statistics(StatisticsImpl inputStat)
{
            Statistics statistics = new Statistics();

            statistics.SessionCloseCount = inputStat.SessionCloseCount;
            statistics.SessionOpenCount = inputStat.SessionOpenCount;

            statistics.CloseStatementCount = inputStat.CloseStatementCount;
            statistics.PrepareStatementCount = inputStat.PrepareStatementCount;

            statistics.EntityFetchCount = inputStat.EntityFetchCount;
            statistics.EntityDeleteCount = inputStat.EntityDeleteCount;
            statistics.EntityInsertCount = inputStat.EntityInsertCount;
            statistics.EntityLoadCount = inputStat.EntityLoadCount;
            statistics.EntityUpdateCount = inputStat.EntityUpdateCount;

            statistics.CollectionFetchCount = inputStat.CollectionFetchCount;
            statistics.CollectionLoadCount = inputStat.CollectionLoadCount;
            statistics.CollectionRecreateCount = inputStat.CollectionRecreateCount;
            statistics.CollectionRemoveCount = inputStat.CollectionRemoveCount;
            statistics.CollectionUpdateCount = inputStat.CollectionUpdateCount;

            statistics.SecondLevelCacheHitCount = inputStat.SecondLevelCacheHitCount;
            statistics.SecondLevelCacheMissCount = inputStat.SecondLevelCacheMissCount;
            statistics.SecondLevelCachePutCount = inputStat.SecondLevelCachePutCount;

            statistics.NumberOfSQLSelectStatement = null;
            statistics.NumberOfSQLInsertStatement = null;
            statistics.NumberOfSQLUpdateStatement = null;
            statistics.NumberOfSQLDeleteStatement = null;

            return statistics;
        }

        internal void UpdateSqlLogSpyStatistics(SqlLogSpyStatistics sqlLogSpyStatistics)
        {
            this.NumberOfSQLSelectStatement = sqlLogSpyStatistics.NumberOfSQLSelectStatement;
            this.NumberOfSQLInsertStatement = sqlLogSpyStatistics.NumberOfSQLInsertStatement;
            this.NumberOfSQLUpdateStatement = sqlLogSpyStatistics.NumberOfSQLUpdateStatement;
            this.NumberOfSQLDeleteStatement = sqlLogSpyStatistics.NumberOfSQLDeleteStatement;
        }
    }

}
