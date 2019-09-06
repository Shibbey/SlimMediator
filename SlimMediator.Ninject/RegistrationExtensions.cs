using Ninject;
using SlimMediator.Registration;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlimMediator
{
    /// <summary>
    /// Extensions to be used with Ninject.
    /// </summary>
    public static class RegistrationExtensions
    {
        #region Public Methods

        /// <summary>
        /// Gets the service registrar.
        /// </summary>
        /// <param name="kernel"> The kernel. </param>
        /// <returns>  </returns>
        public static IServiceRegistrar GetServiceRegistrar(this IKernel kernel)
            => new ServiceRegistrar(kernel);

        /// <summary>
        /// Registers all notification handlers.
        /// </summary>
        /// <param name="kernel">            The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="IKernel" />. </returns>
        public static IKernel RegisterAllNotificationHandlers(this IKernel kernel, RegistrationScope registrationScope, IEnumerable<Assembly> assembliesToScan)
        {
            kernel
                .GetServiceRegistrar()
                .RegisterAllNotificationHandlers(registrationScope, assembliesToScan);

            return kernel;
        }

        /// <summary>
        /// Registers all notification handlers.
        /// </summary>
        /// <param name="kernel">            The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="IKernel" />. </returns>
        public static IKernel RegisterAllNotificationHandlers(this IKernel kernel, RegistrationScope registrationScope, params Assembly[] assembliesToScan)
            => kernel.RegisterAllNotificationHandlers(registrationScope, assembliesToScan.AsEnumerable());

        /// <summary>
        /// Registers all request handlers.
        /// </summary>
        /// <param name="kernel">            The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="IKernel" />. </returns>
        public static IKernel RegisterAllRequestHandlers(this IKernel kernel, RegistrationScope registrationScope, IEnumerable<Assembly> assembliesToScan)
        {
            kernel
                .GetServiceRegistrar()
                .RegisterAllRequestHandlers(registrationScope, assembliesToScan);

            return kernel;
        }

        /// <summary>
        /// Registers all request handlers.
        /// </summary>
        /// <param name="kernel">            The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns> The <see cref="IKernel" />. </returns>
        public static IKernel RegisterAllRequestHandlers(this IKernel kernel, RegistrationScope registrationScope, params Assembly[] assembliesToScan)
            => kernel.RegisterAllRequestHandlers(registrationScope, assembliesToScan.AsEnumerable());

        /// <summary>
        /// Registers the default mediator.
        /// </summary>
        /// <param name="kernel">            The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <returns> The <see cref="IKernel" />. </returns>
        public static IKernel RegisterDefaultMediator(this IKernel kernel, RegistrationScope registrationScope)
        {
            kernel
                .GetServiceRegistrar()
                .RegisterDefaultMediator(registrationScope);

            return kernel;
        }

        #endregion Public Methods
    }
}