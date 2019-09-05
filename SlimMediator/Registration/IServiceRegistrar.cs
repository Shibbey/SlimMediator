using System;

namespace SlimMediator.Registration
{
    /// <summary>
    /// DI ambiguous registration method definitions.
    /// </summary>
    public interface IServiceRegistrar
    {
        #region Public Methods

        /// <summary>
        /// Adds the scoped.
        /// </summary>
        /// <param name="serviceType">        Type of the service. </param>
        /// <param name="implementationType"> Type of the implementation. </param>
        /// <returns> The <see cref="IServiceRegistrar" />. </returns>
        IServiceRegistrar AddScoped(Type serviceType, Type implementationType);

        /// <summary>
        /// Adds the singleton.
        /// </summary>
        /// <param name="serviceType">        Type of the service. </param>
        /// <param name="implementationType"> Type of the implementation. </param>
        /// <returns> The <see cref="IServiceRegistrar" />. </returns>
        IServiceRegistrar AddSingleton(Type serviceType, Type implementationType);

        /// <summary>
        /// Adds the transient.
        /// </summary>
        /// <param name="serviceType">        Type of the service. </param>
        /// <param name="implementationType"> Type of the implementation. </param>
        /// <returns> The <see cref="IServiceRegistrar" />. </returns>
        IServiceRegistrar AddTransient(Type serviceType, Type implementationType);

        #endregion Public Methods
    }
}