using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimMediator.TestItems;
using System;
using System.Linq;

namespace SlimMediator
{
    [TestClass]
    public class CoreRegistrationTests
    {
        #region Public Methods

        [TestMethod]
        public void Core_RegistersAllNotificationHandlers()
        {
            // Arrange
            var sc = new ServiceCollection();

            // Act
            sc.RegisterAllNotificationHandlers(Registration.RegistrationScope.Scoped);
            var sp = sc.BuildServiceProvider();
            var handlers = sp.GetServices<INotificationHandler<TestNotification>>();
            var handlers2 = sp.GetServices<INotificationHandler<TestNotification2>>();
            var handlers3 = sp.GetServices<INotificationHandler<TestNotification3>>();

            // Assert
            sc.Count.Should().BeGreaterOrEqualTo(5);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            handlers.Count().Should().Be(3);
            handlers2.Count().Should().Be(1);
            handlers3.Count().Should().Be(1);
        }

        [TestMethod]
        public void Core_RegistersAllRequestHandlers()
        {
            // Arrange
            var sc = new ServiceCollection();

            // Act
            sc.RegisterAllRequestHandlers(Registration.RegistrationScope.Scoped);
            var sp = sc.BuildServiceProvider();
            Action getHandler = () => sp.GetRequiredService<IRequestHandler<TestRequest, int>>();
            Action getHandler2 = () => sp.GetRequiredService<IRequestHandler<TestRequest2, int>>();

            // Assert
            sc.Count.Should().BeGreaterOrEqualTo(2);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            getHandler.Should().NotThrow();
            getHandler2.Should().NotThrow();
        }

        [TestMethod]
        public void Core_RegistersDefaultmediator_Scoped()
        {
            // Arrange
            var sc = new ServiceCollection();

            // Act
            sc.RegisterDefaultMediator(Registration.RegistrationScope.Scoped);
            var sp = sc.BuildServiceProvider();
            Action getMediator = () => sp.GetRequiredService<IMediator>();

            // Assert
            sc.Count.Should().Be(1);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
            getMediator.Should().NotThrow();
        }

        [TestMethod]
        public void Core_RegistersDefaultmediator_Singleton()
        {
            // Arrange
            var sc = new ServiceCollection();

            // Act
            sc.RegisterDefaultMediator(Registration.RegistrationScope.Singleton);
            var sp = sc.BuildServiceProvider();
            Action getMediator = () => sp.GetRequiredService<IMediator>();

            // Assert
            sc.Count.Should().Be(1);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
            getMediator.Should().NotThrow();
        }

        [TestMethod]
        public void Core_RegistersDefaultmediator_Transient()
        {
            // Arrange
            var sc = new ServiceCollection();

            // Act
            sc.RegisterDefaultMediator(Registration.RegistrationScope.Transient);
            var sp = sc.BuildServiceProvider();
            Action getMediator = () => sp.GetRequiredService<IMediator>();

            // Assert
            sc.Count.Should().Be(1);
            sc.First().Lifetime.Should().Be(ServiceLifetime.Transient);
            getMediator.Should().NotThrow();
        }

        #endregion Public Methods
    }
}