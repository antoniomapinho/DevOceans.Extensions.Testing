using System;
using System.Net.Http;
using NUnit.Framework;

namespace DevOceans.Extensions.Testing.Tests.Unit
{
    [TestFixture]
    public class MockHttpMessageHandlerTests
    {
        [Test]
        public void Setup_WithDelegate_AddsExpectation()
        {
            var handler = new MockHttpMessageHandler();

            var checker = false;

            var expectation = handler.Setup(request => checker = true);

            expectation.RequestExpression.Invoke(new HttpRequestMessage());

            Assert.IsTrue(checker);
        }
    }
}
