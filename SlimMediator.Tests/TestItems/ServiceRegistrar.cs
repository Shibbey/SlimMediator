using Microsoft.Extensions.DependencyInjection;
using SlimMediator.Registration;
using System;

namespace SlimMediator.TestItems
{
    internal class ServiceRegistrar
        : IServiceRegistrar
    {
        #region Private Fields

        private readonly ServiceCollection _services;

        #endregion Private Fields

        #region Public Constructors

        public ServiceRegistrar(ServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        #endregion Public Constructors

        #region Public Methods

        public IServiceRegistrar AddScoped(Type serviceType, Type implementationType)
        {
            _services.AddScoped(serviceType, implementationType);
            return this;
        }

        public IServiceRegistrar AddSingleton(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
            return this;
        }

        public IServiceRegistrar AddTransient(Type serviceType, Type implementationType)
        {
            _services.AddTransient(serviceType, implementationType);
            return this;
        }

        #endregion Public Methods
    }
}