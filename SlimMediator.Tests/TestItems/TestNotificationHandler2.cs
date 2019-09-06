using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator.TestItems
{
    public class TestNotificationHandler2
        : INotificationHandler<TestNotification>
        , INotificationHandler<TestNotification2>
    {
        #region Public Methods

        public Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(TestNotification2 notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}