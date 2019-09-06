using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SlimMediator.TestItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    [TestClass]
    public class NullTests
    {
        #region Public Methods

        [TestMethod]
        public void NotificationHandlerNotRegistered()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IMediator, Mediator>();

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<IMediator>();

            Func<Task> func = async () => await sut.PublishAsync(new TestNotification(), CancellationToken.None);

            func.Should().Throw<InvalidOperationException>()
                .WithMessage("No notification handlers registered for *.");
        }

        [TestMethod]
        public void NullNotification()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IMediator, Mediator>();

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<IMediator>();
            TestNotification notification = null;

            Func<Task> func = async () => await sut.PublishAsync(notification, CancellationToken.None);

            func.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.*Parameter name: notification");
        }

        [TestMethod]
        public void NullRequest()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IMediator, Mediator>();

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<IMediator>();
            TestRequest request = null;

            Func<Task> func = async () => await sut.SendAsync(request, CancellationToken.None);

            func.Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null.*Parameter name: request");
        }

        [TestMethod]
        public void RequestHandlerNotRegistered()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IMediator, Mediator>();

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<IMediator>();
            var request = new TestRequest();

            Func<Task> func = async () => await sut.SendAsync(request, CancellationToken.None);

            func.Should().Throw<InvalidOperationException>()
                .WithMessage("* is not registered.");
        }

        #endregion Public Methods
    }
}