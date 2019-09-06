using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator.TestItems
{
    public class TestNotificationHandler
        : INotificationHandler<TestNotification>
    {
        #region Public Methods

        public Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}