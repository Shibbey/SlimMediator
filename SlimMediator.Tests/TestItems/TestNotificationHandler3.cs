using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator.TestItems
{
    public class TestNotificationHandler3
        : INotificationHandler<TestNotification>
        , INotificationHandler<TestNotification3>
    {
        #region Public Methods

        public Task Handle(TestNotification notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Handle(TestNotification3 notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}