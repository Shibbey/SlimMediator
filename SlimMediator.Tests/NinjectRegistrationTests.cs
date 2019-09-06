using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using SlimMediator.TestItems;
using System;
using System.Linq;

namespace SlimMediator
{
    [TestClass]
    public class NinjectRegistrationTests
    {
        #region Public Methods

        [TestMethod]
        public void Ninject_RegistersAllNotificationHandlers()
        {
            // Arrange
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterAllNotificationHandlers(Registration.RegistrationScope.Scoped);
            var handlers = kernel.GetAll<INotificationHandler<TestNotification>>();
            var handlers2 = kernel.GetAll<INotificationHandler<TestNotification2>>();
            var handlers3 = kernel.GetAll<INotificationHandler<TestNotification3>>();

            // Assert
            handlers.Count().Should().Be(3);
            handlers2.Count().Should().Be(1);
            handlers3.Count().Should().Be(1);
        }

        [TestMethod]
        public void Ninject_RegistersAllRequestHandlers()
        {
            // Arrange
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterAllRequestHandlers(Registration.RegistrationScope.Scoped);
            var handler = kernel.Get<IRequestHandler<TestRequest, int>>();
            var handler2 = kernel.Get<IRequestHandler<TestRequest2, int>>();

            // Assert
            handler.Should().NotBeNull();
            handler2.Should().NotBeNull();
        }

        [TestMethod]
        public void Ninject_RegistersDefaultmediator_Scoped()
        {
            // Arrange
            var kernel = new StandardKernel();
            kernel.Bind<IServiceProvider>().To<NinjectServiceProvider>().InSingletonScope();

            // Act
            kernel.RegisterDefaultMediator(Registration.RegistrationScope.Scoped);
            var mediator = kernel.Get<IMediator>();

            // Assert
            mediator.Should().NotBeNull();
        }

        [TestMethod]
        public void Ninject_RegistersDefaultmediator_Singleton()
        {
            // Arrange
            var kernel = new StandardKernel();
            kernel.Bind<IServiceProvider>().To<NinjectServiceProvider>().InSingletonScope();

            // Act
            kernel.RegisterDefaultMediator(Registration.RegistrationScope.Singleton);
            var mediator = kernel.Get<IMediator>();

            // Assert
            mediator.Should().NotBeNull();
        }

        [TestMethod]
        public void Ninject_RegistersDefaultmediator_Transient()
        {
            // Arrange
            var kernel = new StandardKernel();
            kernel.Bind<IServiceProvider>().To<NinjectServiceProvider>().InSingletonScope();

            // Act
            kernel.RegisterDefaultMediator(Registration.RegistrationScope.Transient);
            var mediator = kernel.Get<IMediator>();

            // Assert
            mediator.Should().NotBeNull();
        }

        #endregion Public Methods
    }
}