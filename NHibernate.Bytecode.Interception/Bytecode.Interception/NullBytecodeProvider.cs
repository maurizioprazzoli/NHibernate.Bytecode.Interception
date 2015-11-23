using NHibernate.Bytecode.Interception.Interceptor;
using NHibernate.Properties;

namespace NHibernate.Bytecode.Interception
{
    /// <summary>
    /// A <see cref="IBytecodeProvider" /> implementation that returns
    /// <see langword="null" />, disabling reflection optimization.
    /// </summary>
    public class NullBytecodeProvider : AbstractBytecodeProvider
    {
        #region ctor
        public NullBytecodeProvider()
            : base() { }

        public NullBytecodeProvider(IBytecodeProviderInterceptor interceptor)
            : base(interceptor) { }
        #endregion

        #region IBytecodeProvider Members

        public override IReflectionOptimizer GetReflectionOptimizer(System.Type clazz, IGetter[] getters, ISetter[] setters)
        {
            return null;
        }

        #endregion
    }
}