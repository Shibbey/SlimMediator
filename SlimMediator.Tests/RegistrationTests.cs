using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    [TestClass]
    public class RegistrationTests
    {
        #region Public Methods

        [TestMethod]
        public void RegistersAllNotificationHandlers()
        {
            var sc = new ServiceCollection();

            // Act
            sc.RegisterAllNotificationHandlers(Registration.RegistrationScope.Scoped);

            // Assert
            sc.Count.Should().BeGreaterOrEqualTo(5);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [TestMethod]
        public void RegistersAllRequestHandlers()
        {
            var sc = new ServiceCollection();

            // Act
            sc.RegisterAllRequestHandlers(Registration.RegistrationScope.Scoped);

            // Assert
            sc.Count.Should().BeGreaterOrEqualTo(2);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [TestMethod]
        public void RegistersDefaultmediator_Scoped()
        {
            var sc = new ServiceCollection();

            // Act
            sc.RegisterDefaultMediator(Registration.RegistrationScope.Scoped);

            // Assert
            sc.Count.Should().Be(1);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [TestMethod]
        public void RegistersDefaultmediator_Singleton()
        {
            var sc = new ServiceCollection();

            // Act
            sc.RegisterDefaultMediator(Registration.RegistrationScope.Singleton);

            // Assert
            sc.Count.Should().Be(1);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
        }

        [TestMethod]
        public void RegistersDefaultmediator_Transient()
        {
            var sc = new ServiceCollection();

            // Act
            sc.RegisterDefaultMediator(Registration.RegistrationScope.Transient);

            // Assert
            sc.Count.Should().Be(1);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Transient);
        }

        #endregion Public Methods

        #region Public Classes

        public class TestNotification
            : INotification
        { }

        public class TestNotification2
            : INotification
        { }

        public class TestNotification3
            : INotification
        { }

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

        #endregion Public Classes
    }
}