﻿using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    /// <summary>
    /// Defines a handler for a notification
    /// </summary>
    /// <typeparam name="TNotification"> The type of notification being handled </typeparam>
    public interface INotificationHandler<in TNotification>
        where TNotification : INotification
    {
        #region Public Methods

        /// <summary>
        /// Handles a notification
        /// </summary>
        /// <param name="notification">      The notification </param>
        /// <param name="cancellationToken"> Cancellation token </param>
        Task Handle(TNotification notification, CancellationToken cancellationToken);

        #endregion Public Methods
    }
}