


using NHibernate.Bytecode.Interception.Interceptor;
using NHibernate.Proxy;
using System;
namespace NHibernate.Bytecode.Interception
{
	public class DefaultProxyFactoryFactory : IProxyFactoryFactory
    {
        private IBytecodeProviderInterceptor interceptor;

        public DefaultProxyFactoryFactory(IBytecodeProviderInterceptor interceptor)
        {
            this.interceptor = interceptor;
        }
        #region ctor

        #endregion

        #region IProxyFactoryFactory Members

        public NHibernate.Proxy.IProxyFactory BuildProxyFactory()
		{
            if (interceptor == null)
                throw new Exception("NHibernate.Bytecode.Interception - Error construncting DefaultProxyFactory. Interceptor istance is null.");

            return new NHibernate.Proxy.Interception.DefaultProxyFactory(interceptor);
		}

		public NHibernate.Proxy.IProxyValidator ProxyValidator
		{
			get { return new DynProxyTypeValidator(); }
		}

		public bool IsInstrumented(System.Type entityClass)
		{
			return true;
		}

		public bool IsProxy(object entity)
		{
			return entity is INHibernateProxy;
		}

		#endregion
	}
}