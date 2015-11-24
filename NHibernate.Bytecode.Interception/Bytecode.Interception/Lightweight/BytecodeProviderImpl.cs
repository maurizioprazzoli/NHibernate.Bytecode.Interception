using NHibernate.Bytecode.Interception.Interceptor;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Properties;

namespace NHibernate.Bytecode.Interception.Lightweight
{
	/// <summary>
	/// Factory that generate object based on IReflectionOptimizer needed to replace the use
	/// of reflection.
	/// </summary>
	/// <remarks>
	/// Used in <see cref="NHibernate.Persister.Entity.AbstractEntityPersister"/> and
	/// <see cref="NHibernate.Type.ComponentType"/>
	/// </remarks>
	public class BytecodeProviderImpl : AbstractBytecodeProvider
	{
         #region ctor
        public BytecodeProviderImpl()
            : base() { }

        public BytecodeProviderImpl(IBytecodeProviderInterceptor interceptor)
            : base(interceptor) { }
        #endregion

		#region IBytecodeProvider Members

		/// <summary>
		/// Generate the IReflectionOptimizer object
		/// </summary>
		/// <param name="mappedClass">The target class</param>
		/// <param name="setters">Array of setters</param>
		/// <param name="getters">Array of getters</param>
		/// <returns><see langword="null" /> if the generation fails</returns>
		public override IReflectionOptimizer GetReflectionOptimizer(System.Type mappedClass, IGetter[] getters, ISetter[] setters)
		{
            return new ReflectionOptimizer(mappedClass, getters, setters, interceptor);
		}

		#endregion
	}
}