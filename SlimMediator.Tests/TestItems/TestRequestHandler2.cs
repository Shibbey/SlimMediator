using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator.TestItems
{
    public class TestRequestHandler2
        : IRequestHandler<TestRequest2, int>
    {
        #region Public Methods

        public Task<int> HandleAsync(TestRequest2 request, CancellationToken cancellationToken)
        {
            return Task.FromResult(2);
        }

        #endregion Public Methods
    }
}