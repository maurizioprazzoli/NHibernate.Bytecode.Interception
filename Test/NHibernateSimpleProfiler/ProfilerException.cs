using System;

namespace NHibernateSimpleProfiler
{
    class NHibernateSimpleProfilerException : Exception
    {
        public NHibernateSimpleProfilerException(string message)
            : base(message)
        { }

        public NHibernateSimpleProfilerException(string message, object arg)
            : base(String.Format(message, arg))
        { }

        public NHibernateSimpleProfilerException(string message, object[] args)
            : base(String.Format(message, args))
        { }
    }
}
