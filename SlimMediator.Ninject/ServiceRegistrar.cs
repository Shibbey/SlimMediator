using Ninject;
using SlimMediator.Registration;
using System;

namespace SlimMediator
{
    internal class ServiceRegistrar
        : IServiceRegistrar
    {
        #region Private Fields

        private readonly IKernel _kernel;

        #endregion Private Fields

        #region Public Constructors

        public ServiceRegistrar(IKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        #endregion Public Constructors

        #region Public Methods

        public IServiceRegistrar AddScoped(Type serviceType, Type implementationType)
        {
            _kernel.Bind(serviceType).To(implementationType).InThreadScope();
            return this;
        }

        public IServiceRegistrar AddSingleton(Type serviceType, Type implementationType)
        {
            _kernel.Bind(serviceType).To(implementationType).InSingletonScope();
            return this;
        }

        public IServiceRegistrar AddTransient(Type serviceType, Type implementationType)
        {
            _kernel.Bind(serviceType).To(implementationType).InTransientScope();
            return this;
        }

        #endregion Public Methods
    }
}