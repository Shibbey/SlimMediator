﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SlimMediator
{
    [TestClass]
    public class SuccessfulSends
    {
        #region Public Methods

        [TestMethod]
        public void HandlerRegistered()
        {
            var sc = new ServiceCollection();
            sc.AddSingleton<IMediator, Mediator>();
            sc.AddSingleton<IRequestHandler<TestRequest, int>, TestRequestHandler>();

            var sp = sc.BuildServiceProvider();
            var sut = sp.GetRequiredService<IMediator>();
            var request = new TestRequest();
            int result = 0;

            Func<Task> func = async () => result = await sut.SendAsync(request, CancellationToken.None);

            func.Should().NotThrow();
            result.Should().Be(1);
        }

        #endregion Public Methods

        #region Private Classes

        private class TestRequest
            : IRequest<int>
        {
        }

        private class TestRequestHandler
            : IRequestHandler<TestRequest, int>
        {
            #region Public Methods

            public Task<int> HandleAsync(TestRequest request, CancellationToken cancellationToken)
            {
                return Task.FromResult(1);
            }

            #endregion Public Methods
        }

        #endregion Private Classes
    }
}