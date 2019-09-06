using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator.TestItems
{
    public class TestRequestHandler
        : IRequestHandler<TestRequest, int>
    {
        #region Public Methods

        public Task<int> HandleAsync(TestRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(1);
        }

        #endregion Public Methods
    }
}