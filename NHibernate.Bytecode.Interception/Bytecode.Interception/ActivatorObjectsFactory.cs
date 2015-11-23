using NHibernate.Bytecode.Interception.Interceptor;
using System;

namespace NHibernate.Bytecode.Interception
{
	public class ActivatorObjectsFactory: IObjectsFactory
    {
        IBytecodeProviderInterceptor interceptor;
        #region ctor
        public ActivatorObjectsFactory(IBytecodeProviderInterceptor interceptor)
        {
            this.interceptor = interceptor;
        }
        #endregion
        public object CreateInstance(System.Type type)
		{
            object instance = interceptor.CreateInstance(type);
            if (instance == null)
            {
                return Activator.CreateInstance(type);
            }
            return instance;
		}

		public object CreateInstance(System.Type type, bool nonPublic)
		{
            object instance = interceptor.CreateInstance(type, nonPublic);
            if (instance == null)
            {
            return Activator.CreateInstance(type, nonPublic);
            }
            return instance;
		}

		public object CreateInstance(System.Type type, params object[] ctorArgs)
		{
            object instance = interceptor.CreateInstance(type, ctorArgs);
             if (instance == null)
             {
                 return Activator.CreateInstance(type, ctorArgs);
             }
             return instance;
		}
	}
}