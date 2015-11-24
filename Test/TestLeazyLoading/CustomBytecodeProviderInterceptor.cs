using NHibernate.Bytecode.Interception;

namespace TestLeazyLoading
{
    class CustomBytecodeProviderInterceptor : EmptyBytecodeProviderInterceptor
    {
        public override object CreateInstance(System.Type type)
        {
            CustomBytecodeProviderInterceptorCounter.CreateInstance++;
            incrementTypeCreationCounter(type);
            return null;
        }

        public override object CreateInstance(System.Type type, bool nonPublic)
        {
            CustomBytecodeProviderInterceptorCounter.CreateInstance++;
            incrementTypeCreationCounter(type);
            return null;
        }

        public override object CreateInstance(System.Type type, object[] ctorArgs)
        {
            CustomBytecodeProviderInterceptorCounter.CreateInstance++;
            incrementTypeCreationCounter(type);
            return null;
        }

        public override object CreateProxyInstance(System.Type proxyType)
        {
            CustomBytecodeProviderInterceptorCounter.CreateProxyInstance++;
            incrementTypeCreationCounter(proxyType);
            return null;
        }

        public override object CreateProxyInstance(System.Type proxyType, object[] args)
        {
            CustomBytecodeProviderInterceptorCounter.CreateProxyInstance++;
            incrementTypeCreationCounter(proxyType);
            return null;
        }

        public override void ProxyTypeCreated(System.Type proxyType)
        {
            CustomBytecodeProviderInterceptorCounter.ProxyTypeCreated++;
        }

        private void incrementTypeCreationCounter(System.Type objType)
        {
            switch (objType.ToString())
            {
                case "Model.Item":
                    CustomBytecodeProviderInterceptorCounter.NewItemCallIntecepted++;
                    break;
                case "Model.Bid":
                    CustomBytecodeProviderInterceptorCounter.NewBidCallIntecepted++;
                    break;
                case "ItemProxy":
                    CustomBytecodeProviderInterceptorCounter.NewItemProxyCallIntecepted++;
                    break;
                case "BidProxy":
                    CustomBytecodeProviderInterceptorCounter.NewBidProxyCallIntecepted++;
                    break;
            }
        }
    }
}
