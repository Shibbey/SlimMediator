using SlimMediator.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SlimMediator
{
    /// <summary>
    /// Extensions to be used for DI
    /// </summary>
    public static class RegistrationExtensions
    {
        #region Public Methods

        /// <summary>
        /// Registers all notification handlers.
        /// </summary>
        /// <param name="serviceRegistrar">  The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns>  </returns>
        public static IServiceRegistrar RegisterAllNotificationHandlers(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope, IEnumerable<Assembly> assembliesToScan)
        {
            assembliesToScan = (assembliesToScan as Assembly[] ?? assembliesToScan).Distinct().ToArray();
            registrationScope = RegistrationScope.Singleton.Restricted(registrationScope);

            return serviceRegistrar
                .ConnectImplementationsToTypesClosing(registrationScope, typeof(INotificationHandler<>), assembliesToScan, true);
        }

        /// <summary>
        /// Registers all notification handlers.
        /// </summary>
        /// <param name="serviceRegistrar">  The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns>  </returns>
        public static IServiceRegistrar RegisterAllNotificationHandlers(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope, params Assembly[] assembliesToScan)
            => serviceRegistrar.RegisterAllNotificationHandlers(registrationScope, assembliesToScan.AsEnumerable());

        /// <summary>
        /// Registers all request handlers.
        /// </summary>
        /// <param name="serviceRegistrar">  The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns>  </returns>
        public static IServiceRegistrar RegisterAllRequestHandlers(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope, IEnumerable<Assembly> assembliesToScan)
        {
            assembliesToScan = (assembliesToScan as Assembly[] ?? assembliesToScan).Distinct().ToArray();
            registrationScope = RegistrationScope.Singleton.Restricted(registrationScope);

            return serviceRegistrar
                .ConnectImplementationsToTypesClosing(registrationScope, typeof(IRequestHandler<,>), assembliesToScan, false);
        }

        /// <summary>
        /// Registers all request handlers.
        /// </summary>
        /// <param name="serviceRegistrar">  The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <param name="assembliesToScan">  The assemblies to scan. </param>
        /// <returns>  </returns>
        public static IServiceRegistrar RegisterAllRequestHandlers(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope, params Assembly[] assembliesToScan)
            => serviceRegistrar.RegisterAllRequestHandlers(registrationScope, assembliesToScan.AsEnumerable());

        /// <summary>
        /// Registers the default mediator.
        /// </summary>
        /// <param name="serviceRegistrar">  The service registrar. </param>
        /// <param name="registrationScope"> The registration scope. </param>
        /// <returns> The <see cref="IServiceRegistrar" /> </returns>
        public static IServiceRegistrar RegisterDefaultMediator(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope)
        {
            registrationScope = RegistrationScope.Singleton.Restricted(registrationScope);
            return serviceRegistrar
                .Register<IMediator, Mediator>(registrationScope);
        }

        #endregion Public Methods

        #region Private Methods

        private static void AddConcretionsThatCouldBeClosed(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope, Type @interface, List<Type> concretions)
        {
            foreach (var type in concretions
                .Where(x => x.IsOpenGeneric() && x.CouldCloseTo(@interface)))
            {
                try
                {
                    serviceRegistrar.Register(@interface, type.MakeGenericType(@interface.GenericTypeArguments), registrationScope);
                }
                catch (Exception)
                {
                }
            }
        }

        private static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null) return false;

            if (pluggedType == pluginType) return true;

            return pluginType.GetTypeInfo().IsAssignableFrom(pluggedType.GetTypeInfo());
        }

        /// <summary>
        /// Helper method use to differentiate behavior between request handlers and notification
        /// handlers. Request handlers should only be added once (so set addIfAlreadyExists to false)
        /// Notification handlers should all be added (set addIfAlreadyExists to true)
        /// </summary>
        /// <param name="serviceRegistrar">      </param>
        /// <param name="registrationScope">     </param>
        /// <param name="openRequestInterface">  </param>
        /// <param name="assembliesToScan">      </param>
        /// <param name="addIfAlreadyExists">    </param>
        private static IServiceRegistrar ConnectImplementationsToTypesClosing(this IServiceRegistrar serviceRegistrar,
                                                                 RegistrationScope registrationScope,
                                                                 Type openRequestInterface,
                                                                 IEnumerable<Assembly> assembliesToScan,
                                                                 bool addIfAlreadyExists)
        {
            if (!assembliesToScan.Any())
            {
                assembliesToScan = AppDomain.CurrentDomain.GetAssemblies();
            }

            var concretions = new List<Type>();
            var interfaces = new List<Type>();
            foreach (var type in assembliesToScan.SelectMany(a => a.DefinedTypes).Where(t => !t.IsOpenGeneric()))
            {
                var interfaceTypes = Enumerable.ToArray<Type>(type.FindInterfacesThatClose(openRequestInterface));
                if (!interfaceTypes.Any()) continue;

                if (type.IsConcrete())
                {
                    concretions.Add(type);
                }

                foreach (var interfaceType in interfaceTypes)
                {
                    interfaces.Fill(interfaceType);
                }
            }

            foreach (var @interface in interfaces)
            {
                var exactMatches = concretions.Where(x => x.CanBeCastTo(@interface)).ToList();
                if (addIfAlreadyExists)
                {
                    foreach (var type in exactMatches)
                    {
                        serviceRegistrar.Register(@interface, type, registrationScope);
                    }
                }
                else
                {
                    if (exactMatches.Count > 1)
                    {
                        exactMatches.RemoveAll(m => !IsMatchingWithInterface(m, @interface));
                    }

                    foreach (var type in exactMatches)
                    {
                        serviceRegistrar.Register(@interface, type, registrationScope);
                    }
                }

                if (!@interface.IsOpenGeneric())
                {
                    serviceRegistrar.AddConcretionsThatCouldBeClosed(registrationScope, @interface, concretions);
                }
            }

            return serviceRegistrar;
        }

        private static bool CouldCloseTo(this Type openConcretion, Type closedInterface)
        {
            var openInterface = closedInterface.GetGenericTypeDefinition();
            var arguments = closedInterface.GenericTypeArguments;

            var concreteArguments = openConcretion.GenericTypeArguments;
            return arguments.Length == concreteArguments.Length && openConcretion.CanBeCastTo(openInterface);
        }

        private static void Fill<T>(this IList<T> list, T value)
        {
            if (list.Contains(value)) return;
            list.Add(value);
        }

        private static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            return Enumerable.Distinct<Type>(FindInterfacesThatClosesCore(pluggedType, templateType));
        }

        private static IEnumerable<Type> FindInterfacesThatClosesCore(Type pluggedType, Type templateType)
        {
            if (pluggedType == null) yield break;

            if (!pluggedType.IsConcrete()) yield break;

            if (templateType.GetTypeInfo().IsInterface)
            {
                foreach (
                    var interfaceType in
                    pluggedType.GetInterfaces()
                        .Where(type => type.GetTypeInfo().IsGenericType && (type.GetGenericTypeDefinition() == templateType)))
                {
                    yield return interfaceType;
                }
            }
            else if (pluggedType.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType &&
                     (pluggedType.GetTypeInfo().BaseType.GetGenericTypeDefinition() == templateType))
            {
                yield return pluggedType.GetTypeInfo().BaseType;
            }

            if (pluggedType.GetTypeInfo().BaseType == typeof(object)) yield break;

            foreach (var interfaceType in FindInterfacesThatClosesCore(pluggedType.GetTypeInfo().BaseType, templateType))
            {
                yield return interfaceType;
            }
        }

        private static bool IsConcrete(this Type type)
        {
            return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
        }

        private static bool IsMatchingWithInterface(Type handlerType, Type handlerInterface)
        {
            if (handlerType == null || handlerInterface == null)
            {
                return false;
            }

            if (handlerType.IsInterface)
            {
                if (handlerType.GenericTypeArguments.SequenceEqual(handlerInterface.GenericTypeArguments))
                {
                    return true;
                }
            }
            else
            {
                return IsMatchingWithInterface(handlerType.GetInterface(handlerInterface.Name), handlerInterface);
            }

            return false;
        }

        private static bool IsOpenGeneric(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition || type.GetTypeInfo().ContainsGenericParameters;
        }

        private static IServiceRegistrar Register<TService, TImplementation>(this IServiceRegistrar serviceRegistrar, RegistrationScope registrationScope) where TImplementation : TService
            => serviceRegistrar.Register(typeof(TService), typeof(TImplementation), registrationScope);

        private static IServiceRegistrar Register(this IServiceRegistrar serviceRegistrar, Type serviceType, Type implementationType, RegistrationScope registrationScope)
        {
            switch (registrationScope)
            {
                case RegistrationScope.Singleton:
                    return serviceRegistrar.AddSingleton(serviceType, implementationType);

                case RegistrationScope.Scoped:
                    return serviceRegistrar.AddScoped(serviceType, implementationType);

                case RegistrationScope.Transient:
                    return serviceRegistrar.AddTransient(serviceType, implementationType);

                default:
                    break;
            }

            return serviceRegistrar;
        }

        private static RegistrationScope Restricted(this RegistrationScope targetedScope, RegistrationScope limitedScope)
        {
            if (targetedScope == RegistrationScope.Transient || limitedScope == RegistrationScope.Transient)
            {
                return RegistrationScope.Transient;
            }

            if (targetedScope == RegistrationScope.Scoped || limitedScope == RegistrationScope.Scoped)
            {
                return RegistrationScope.Scoped;
            }

            return targetedScope;
        }

        #endregion Private Methods
    }
}