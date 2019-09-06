using Ninject;
using System;

namespace SlimMediator.TestItems
{
    internal class NinjectServiceProvider
        : IServiceProvider
    {
        #region Private Fields

        private readonly IKernel _kernel;

        #endregion Private Fields

        #region Public Constructors

        public NinjectServiceProvider(IKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        #endregion Public Constructors

        #region Public Methods

        public object GetService(Type serviceType)
            => _kernel.Get(serviceType);

        #endregion Public Methods
    }
}