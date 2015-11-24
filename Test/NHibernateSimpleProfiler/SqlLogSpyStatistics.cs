
using NHibernate.Stat;
namespace NHibernateSimpleProfiler
{

    public struct SqlLogSpyStatistics
    {
        public long NumberOfSQLSelectStatement;
        public long NumberOfSQLInsertStatement;
        public long NumberOfSQLUpdateStatement;
        public long NumberOfSQLDeleteStatement;

        public static SqlLogSpyStatistics operator -(SqlLogSpyStatistics left, SqlLogSpyStatistics right)
        {
            SqlLogSpyStatistics sqlLogSpyStatistics = new SqlLogSpyStatistics();

            sqlLogSpyStatistics.NumberOfSQLSelectStatement = left.NumberOfSQLSelectStatement - right.NumberOfSQLSelectStatement;
            sqlLogSpyStatistics.NumberOfSQLInsertStatement = left.NumberOfSQLInsertStatement - right.NumberOfSQLInsertStatement;
            sqlLogSpyStatistics.NumberOfSQLUpdateStatement = left.NumberOfSQLUpdateStatement - right.NumberOfSQLUpdateStatement;
            sqlLogSpyStatistics.NumberOfSQLDeleteStatement = left.NumberOfSQLDeleteStatement - right.NumberOfSQLDeleteStatement;

            return sqlLogSpyStatistics;
        }
    }

}
