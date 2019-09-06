using Microsoft.Extensions.DependencyInjection;
using SlimMediator.Registration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlimMediator
{
    /// <summary>
    /// Extensions to be used with Microsoft.Extensions.DependencyInjection
    /// </summary>
    public static class RegistrationExtensions
    {
        #region Public Methods

        /// <summary>
        /// Gets the service registrar.
        /// </summary>
        /// <param name="services"> The services. </param>
        /// <returns>  </returns>
        public static IServiceRegistrar GetServiceRegistrar(this ServiceCollection services)
            => new ServiceRegistrar(services);

        /// <summary>
        /// Registers all notification handlers.
        /// </summary>
        /// <param name="services">          The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="ServiceCollection" />. </returns>
        public static ServiceCollection RegisterAllNotificationHandlers(this ServiceCollection services, RegistrationScope registrationScope, IEnumerable<Assembly> assembliesToScan)
        {
            services
                .GetServiceRegistrar()
                .RegisterAllNotificationHandlers(registrationScope, assembliesToScan);

            return services;
        }

        /// <summary>
        /// Registers all notification handlers.
        /// </summary>
        /// <param name="services">          The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="ServiceCollection" />. </returns>
        public static ServiceCollection RegisterAllNotificationHandlers(this ServiceCollection services, RegistrationScope registrationScope, params Assembly[] assembliesToScan)
            => services.RegisterAllNotificationHandlers(registrationScope, assembliesToScan.AsEnumerable());

        /// <summary>
        /// Registers all request handlers.
        /// </summary>
        /// <param name="services">          The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="ServiceCollection" />. </returns>
        public static ServiceCollection RegisterAllRequestHandlers(this ServiceCollection services, RegistrationScope registrationScope, IEnumerable<Assembly> assembliesToScan)
        {
            services
                .GetServiceRegistrar()
                .RegisterAllRequestHandlers(registrationScope, assembliesToScan);

            return services;
        }

        /// <summary>
        /// Registers all request handlers.
        /// </summary>
        /// <param name="services">          The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="ServiceCollection" />. </returns>
        public static ServiceCollection RegisterAllRequestHandlers(this ServiceCollection services, RegistrationScope registrationScope, params Assembly[] assembliesToScan)
            => services.RegisterAllRequestHandlers(registrationScope, assembliesToScan.AsEnumerable());

        /// <summary>
        /// Registers the default mediator.
        /// </summary>
        /// <param name="services">          The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <returns> The <see cref="ServiceCollection" />. </returns>
        public static ServiceCollection RegisterDefaultMediator(this ServiceCollection services, RegistrationScope registrationScope)
        {
            services
                .GetServiceRegistrar()
                .RegisterDefaultMediator(registrationScope);

            return services;
        }

        #endregion Public Methods
    }
}