using NHibernate.Bytecode.Interception;

namespace TestInterceptionLeazyLoading.Interception
{
    class CustomBytecodeProviderInterceptor : EmptyBytecodeProviderInterceptor
    {
        public override object CreateInstance(System.Type type)
        {
            return null;
        }

        public override object CreateInstance(System.Type type, bool nonPublic)
        {
            return null;
        }

        public override object CreateInstance(System.Type type, object[] ctorArgs)
        {
            return null;
        }

        public override object CreateProxyInstance(System.Type type)
        {
            return null;
        }

        public override object CreateProxyInstance(System.Type proxyType, object[] args)
        {
            return null;
        }

        public override void ProxyTypeCreated(System.Type proxyType)
        {

        }
    }
}
