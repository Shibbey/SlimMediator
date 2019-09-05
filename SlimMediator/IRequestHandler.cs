using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    /// <summary>
    /// Defines a handler for a request.
    /// </summary>
    /// <typeparam name="TRequest"> The type of request being handled </typeparam>
    /// <typeparam name="TResponse"> The type of response from the handler </typeparam>
    public interface IRequestHandler<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        #region Public Methods

        /// <summary>
        /// Handles the request asynchronously.
        /// </summary>
        /// <param name="request">           The request. </param>
        /// <param name="cancellationToken"> The cancellation token. </param>
        /// <returns> A <see cref="Task{TResult}" /> containing the handler's response. </returns>
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);

        #endregion Public Methods
    }
}