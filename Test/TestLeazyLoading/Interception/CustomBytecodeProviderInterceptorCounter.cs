using System;

namespace TestInterceptionLeazyLoading.Interception
{
    public static class CustomBytecodeProviderInterceptorCounter
    {
        public static Int32 CreateInstance { get; set; }

        public static Int32 CreateProxyInstance { get; set; }

        public static Int32 ProxyTypeCreated { get; set; }

        public static void ResetCounter()
        {
            CreateInstance = 0;
            CreateProxyInstance = 0;
            ProxyTypeCreated = 0;
        }

       
    }
}
