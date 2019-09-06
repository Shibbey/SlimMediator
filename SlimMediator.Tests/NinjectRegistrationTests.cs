using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ninject;

namespace SlimMediator
{
    [TestClass]
    public class NinjectRegistrationTests
    {
        [TestMethod]
        public void RegistersAllNotificationHandlers()
        {
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterAllNotificationHandlers(Registration.RegistrationScope.Scoped);

            // Assert
            //kernel.Components..Count.Should().BeGreaterOrEqualTo(5);
            //kernel.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [TestMethod]
        public void RegistersAllRequestHandlers()
        {
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterAllRequestHandlers(Registration.RegistrationScope.Scoped);

            // Assert
            //sc.Count.Should().BeGreaterOrEqualTo(2);
            //sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [TestMethod]
        public void RegistersDefaultmediator_Scoped()
        {
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterDefaultMediator(Registration.RegistrationScope.Scoped);

            // Assert
            //sc.Count.Should().Be(1);
            //sc.First().Lifetime.Should().Be(ServiceLifetime.Scoped);
        }

        [TestMethod]
        public void RegistersDefaultmediator_Singleton()
        {
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterDefaultMediator(Registration.RegistrationScope.Singleton);

            // Assert
            //sc.Count.Should().Be(1);
            //sc.First().Lifetime.Should().Be(ServiceLifetime.Singleton);
        }

        [TestMethod]
        public void RegistersDefaultmediator_Transient()
        {
            var kernel = new StandardKernel();

            // Act
            kernel.RegisterDefaultMediator(Registration.RegistrationScope.Transient);

            // Assert
            //sc.Count.Should().Be(1);
            //sc.First().Lifetime.Should().Be(ServiceLifetime.Transient);
        }
    }
}
