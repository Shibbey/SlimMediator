using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    /// <summary>
    /// Default mediator implementation.
    /// </summary>
    /// <seealso cref="IMediator" />
    public class Mediator
        : IMediator
    {
        #region Private Fields

        private static readonly ConcurrentDictionary<Type, Lazy<MethodInfo>> _methods = new ConcurrentDictionary<Type, Lazy<MethodInfo>>();
        private readonly IServiceProvider _serviceProvider;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Mediator" /> class.
        /// </summary>
        /// <param name="serviceProvider"> The service provider. </param>
        /// <exception cref="ArgumentNullException"> serviceProvider </exception>
        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Asynchronously send a notification to multiple handlers
        /// </summary>
        /// <typeparam name="TNotification"> The type of the notification. </typeparam>
        /// <param name="notification">      The notification. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A <see cref="Task" /> that represents the publish operation. </returns>
        /// <exception cref="ArgumentNullException"> notification </exception>
        public Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
            where TNotification : INotification
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var handlers = GetHandlers<TNotification>();

            return PublishCore(handlers, notification, cancellationToken);
        }

        /// <summary>
        /// Asynchronously send a request to a single handler.
        /// </summary>
        /// <typeparam name="TResponse"> The type of the response. </typeparam>
        /// <param name="request">           The request. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A <see cref="Task{TResult}" /> containing the handler's response. </returns>
        /// <exception cref="ArgumentNullException"> request </exception>
        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var handler = GetHandler(request);
            var method = GetHandlerMethodInfo<TResponse>(handler);

            return (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken });
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Override in a derived class to control how the tasks are awaited. By default the
        /// implementation is a foreach and await of each handler
        /// </summary>
        /// <param name="allHandlers"> 
        /// Enumerable of tasks representing invoking each notification handler
        /// </param>
        /// <returns> A task representing invoking all handlers </returns>
        protected virtual async Task PublishCore(IEnumerable<Func<Task>> allHandlers)
        {
            foreach (var handler in allHandlers)
            {
                await handler().ConfigureAwait(false);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private object GetHandler<TResponse>(IRequest<TResponse> request)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

            return GetHandler(handlerType);
        }

        private object GetHandler(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            var service = _serviceProvider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException($"{serviceType} is not registered.");
            }

            return service;
        }

        private MethodInfo GetHandlerMethodInfo<TResponse>(object handler)
        {
            var handlerType = handler.GetType();

            var lazy = new Lazy<MethodInfo>(() => handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync)), LazyThreadSafetyMode.ExecutionAndPublication);
            var method = _methods.GetOrAdd(handlerType, lazy).Value;

            if (method == null)
            {
                throw new InvalidOperationException($"{handlerType.Name} is not a known IRequestHandler");
            }

            return method;
        }

        private IEnumerable<INotificationHandler<TNotification>> GetHandlers<TNotification>()
            where TNotification : INotification
        {
            var services = _serviceProvider.GetService(typeof(IEnumerable<INotificationHandler<TNotification>>));
            var result = (IEnumerable<INotificationHandler<TNotification>>)services;

            if (!result.Any())
            {
                throw new InvalidOperationException($"No notification handlers registered for {typeof(TNotification)}.");
            }

            return result;
        }

        private Task PublishCore<TNotification>(IEnumerable<INotificationHandler<TNotification>> allHandlers, TNotification notification, CancellationToken cancellationToken)
                                            where TNotification : INotification
        {
            var functions = allHandlers
                .Select(x => new Func<Task>(() => x.Handle(notification, cancellationToken)));

            return PublishCore(functions);
        }

        #endregion Private Methods
    }
}