using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Bytecode.Interception.Interceptor
{
    public interface IBytecodeProviderInterceptor
    {
        object CreateInstance(System.Type type);

        object CreateInstance(System.Type type, bool nonPublic);

        object CreateInstance(System.Type type, object[] ctorArgs);

        object CreateProxyInstance(System.Type type);

        object CreateProxyInstance(System.Type proxyType, object[] args);

        void ProxyTypeCreated(System.Type proxyType);
    }
}
