using NHibernate;
using NHibernate.Stat;
using System.Collections.Generic;

namespace NHibernateSimpleProfiler
{
    public static class Profiler
    {
        private static ISessionFactory sessionFactory;
        private static Stack<Statistics> snapshotStack;

        public static ISessionFactory SetSessionFactory
        {
            set
            {
                if (value == null || !value.Statistics.IsStatisticsEnabled)
                {
                    throw new NHibernateSimpleProfilerException("NHibernate session factory has not statistics enabled, please anable befor call this. See generate_statistics");
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
            snapshotStack.Push(currentStatisticsSnapshot);
        }

        public static Statistics GetDifferenceFromLastSnapshot()
        {
            if (snapshotStack.Count >= 1)
            {

            }
            Statistics currentStatisticsSnapshot = (Statistics)(StatisticsImpl)sessionFactory.Statistics;
            return currentStatisticsSnapshot - snapshotStack.Peek();
        }
    }
}
