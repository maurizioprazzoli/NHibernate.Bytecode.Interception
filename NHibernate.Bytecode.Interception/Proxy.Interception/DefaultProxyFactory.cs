using System;
using NHibernate.Engine;
using NHibernate.Proxy.Interception.DynamicProxy;
using NHibernate.Bytecode.Interception.Interceptor;
using NHibernate.Intercept;


namespace NHibernate.Proxy.Interception
{
    public class DefaultProxyFactory : AbstractProxyFactory
    {
        private readonly ProxyFactory factory;
        protected static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(DefaultProxyFactory));
        private readonly IBytecodeProviderInterceptor interceptor;

        #region ctor
        public DefaultProxyFactory(IBytecodeProviderInterceptor interceptor)
        {
            this.interceptor = interceptor;
            factory = new ProxyFactory(interceptor);
        }
        #endregion

        public override INHibernateProxy GetProxy(object id, ISessionImplementor session)
        {
            try
            {
                var initializer = new DefaultLazyInitializer(EntityName, PersistentClass, id, GetIdentifierMethod, SetIdentifierMethod, ComponentIdType, session);

                object proxyInstance = IsClassProxy
                                        ? factory.CreateProxy(PersistentClass, initializer, Interfaces)
                                        : factory.CreateProxy(Interfaces[0], initializer, Interfaces);

                return (INHibernateProxy)proxyInstance;
            }
            catch (Exception ex)
            {
                log.Error("Creating a proxy instance failed", ex);
                throw new HibernateException("Creating a proxy instance failed", ex);
            }
        }

        public override object GetFieldInterceptionProxy(object instanceToWrap)
        {
            var interceptor = new DefaultDynamicLazyFieldInterceptor();
            return factory.CreateProxy(PersistentClass, interceptor, new[] { typeof(IFieldInterceptorAccessor) });
        }
    }
}