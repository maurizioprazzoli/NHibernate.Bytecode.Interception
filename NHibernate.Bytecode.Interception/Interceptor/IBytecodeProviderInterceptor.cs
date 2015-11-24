using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Bytecode.Interception.Interceptor
{
    /// <summary>
    /// Allows user code to inspect and/or change instance of entity/proxy required from BytecodeProvider
    /// </summary>
    public interface IBytecodeProviderInterceptor
    {
        /// <summary>
        /// BytecodeProvider call this method when need to instantiate a new class of specific type.
        /// </summary>
        /// <param name="type">Type of class to instantiate</param>
        /// <returns>Instance of class or null for let BytecodeProvider to instantiate</returns>
        object CreateInstance(System.Type type);

        /// <summary>
        /// BytecodeProvider call this method when need to instantiate a new class of specific type.
        /// </summary>
        /// <param name="type">Type of class to instantiate</param>
        /// <param name="nonPublic">true if a public or nonpublic default constructor can match; false if only a public default constructor can match.</param>
        /// <returns></returns>
        object CreateInstance(System.Type type, bool nonPublic);

        /// <summary>
        /// BytecodeProvider call this method when need to instantiate a new class of specific type.
        /// </summary>
        /// <param name="type">Type of class to instantiate</param>
        /// <param name="ctorArgs">Array of constructor arguments</param>
        /// <returns>Instance of class or null for let BytecodeProvider to instantiate</returns>
        object CreateInstance(System.Type type, object[] ctorArgs);

        /// <summary>
        /// BytecodeProvider call this method when need to instantiate a new proxy class of specific type.
        /// </summary>
        /// <param name="type">Type of proxy class to instantiate</param>
        /// <returns>Instance of proxy class or null for let BytecodeProvider to instantiate</returns>
        object CreateProxyInstance(System.Type type);

       /// <summary>
        /// BytecodeProvider call this method when a new proxy class has been created.
       /// </summary>
       /// <param name="proxyType">Type of proxy class created</param>
        void ProxyTypeCreated(System.Type proxyType);
    }
}
