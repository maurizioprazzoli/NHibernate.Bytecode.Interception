using NHibernate.Bytecode.Interception;

namespace TestInterceptionLeazyLoading.Interception
{
    class CustomBytecodeProviderInterceptor : EmptyBytecodeProviderInterceptor
    {
        public override object CreateInstance(System.Type type)
        {
            CustomBytecodeProviderInterceptorCounter.CreateInstance++;
            return null;
        }

        public override object CreateInstance(System.Type type, bool nonPublic)
        {
            CustomBytecodeProviderInterceptorCounter.CreateInstance++;
            return null;
        }

        public override object CreateInstance(System.Type type, object[] ctorArgs)
        {
            CustomBytecodeProviderInterceptorCounter.CreateInstance++;
            return null;
        }

        public override object CreateProxyInstance(System.Type type)
        {
            CustomBytecodeProviderInterceptorCounter.CreateProxyInstance++;
            return null;
        }

        public override object CreateProxyInstance(System.Type proxyType, object[] args)
        {
            CustomBytecodeProviderInterceptorCounter.CreateProxyInstance++;
            return null;
        }

        public override void ProxyTypeCreated(System.Type proxyType)
        {
            CustomBytecodeProviderInterceptorCounter.ProxyTypeCreated++;
        }
    }
}
