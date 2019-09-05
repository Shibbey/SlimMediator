using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    [TestClass]
    public class SuccessfulPublishes
    {
        #region Public Methods

        [TestMethod]
        public void HandlerRegistered()
        {
            var handler1 = CreateMock();
            var handler2 = CreateMock();
            var handler3 = CreateMock();

            var sc = new ServiceCollection();
            sc.AddSingleton<IMediator, Mediator>();
            sc.AddSingleton(handler1.Object);
            sc.AddSingleton(handler2.Object);
            sc.AddSingleton(handler3.Object);

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<IMediator>();

            Func<Task> func = async () => await sut.PublishAsync(new TestNotification(), CancellationToken.None);

            func.Should().NotThrow();
            handler1.Verify(m => m.Handle(It.IsAny<TestNotification>(), It.IsAny<CancellationToken>()), Times.Once);
            handler2.Verify(m => m.Handle(It.IsAny<TestNotification>(), It.IsAny<CancellationToken>()), Times.Once);
            handler3.Verify(m => m.Handle(It.IsAny<TestNotification>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion Public Methods

        #region Private Methods

        private Mock<INotificationHandler<TestNotification>> CreateMock()
        {
            var mock = new Mock<INotificationHandler<TestNotification>>();
            mock.SetReturnsDefault(Task.CompletedTask);
            return mock;
        }

        #endregion Private Methods

        #region Public Classes

        public class TestNotification
            : INotification
        {
        }

        #endregion Public Classes
    }
}