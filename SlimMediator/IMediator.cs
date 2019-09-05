using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    /// <summary>
    /// Defines a mediator to encapsulate request/response and publishing interaction patterns
    /// </summary>
    public interface IMediator
    {
        #region Public Methods

        /// <summary>
        /// Asynchronously send a notification to multiple handlers
        /// </summary>
        /// <typeparam name="TNotification"> The type of the notification. </typeparam>
        /// <param name="notification">      The notification. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A <see cref="Task" /> that represents the publish operation. </returns>
        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken)
            where TNotification : INotification;

        /// <summary>
        /// Asynchronously send a request to a single handler.
        /// </summary>
        /// <typeparam name="TResponse"> The type of the response. </typeparam>
        /// <param name="request">           The request. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A <see cref="Task{TResult}" /> containing the handler's response. </returns>
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);

        #endregion Public Methods
    }
}