using System;

namespace TestNoLeazyLoading
{
    public static class CustomBytecodeProviderInterceptorCounter
    {
        public static Int32 CreateInstance { get; set; }

        public static Int32 CreateProxyInstance { get; set; }

        public static Int32 ProxyTypeCreated { get; set; }

        public static Int32 NewItemCallIntecepted { get; set; }

        public static Int32 NewBidCallIntecepted { get; set; }

        public static Int32 NewItemProxyCallIntecepted { get; set; }

        public static Int32 NewBidProxyCallIntecepted { get; set; }

        public static void ResetCounter()
        {
            CreateInstance = 0;
            CreateProxyInstance = 0;
            ProxyTypeCreated = 0;
            NewItemCallIntecepted = 0;
            NewBidCallIntecepted = 0;
            NewItemProxyCallIntecepted = 0;
            NewBidProxyCallIntecepted = 0;
        }
    }
}
