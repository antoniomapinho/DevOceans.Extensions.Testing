using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DevOceans.Extensions.Testing.Tests.Unit
{
    [TestFixture]
    public class MockHttpMessageHandlerTests
    {
        private HandlerTestTarget _handler;

        [SetUp]
        public void Setup()
        {
            _handler = new HandlerTestTarget();
        }

        [Test]
        public void Setup_WithDelegate_AddsExpectation()
        {
            var checker = false;

            var expectation = _handler.Setup(request => checker = true);

            expectation.RequestExpression.Invoke(new HttpRequestMessage());

            Assert.IsTrue(checker);
        }

        [Test]
        public async Task SendAsync_MultipleMatchingExpectations_ExecutesLastAdded()
        {
            var firstExpectation = new Expectation(
                request => true, 
                request => new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("FirstExpectationResponse")
            });

            var secondExpectation = new Expectation(
                request => true, 
                request => new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent("SecondExpectationResponse")
            });

            var handler = new HandlerTestTarget();

            handler.Expectations.Add(firstExpectation);
            handler.Expectations.Add(secondExpectation);

            var response = await handler.TestSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost"),
                CancellationToken.None);

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Content, Is.Not.Null);
            
            var content = await response.Content.ReadAsStringAsync();

            Assert.That(content.Equals("SecondExpectationResponse"));
        }

        [Test]
        public async Task SendAsync_NoMatchingExpectationsExecutesLast_ReturnsNull()
        {
            var handler = new HandlerTestTarget();

            var response = await handler.TestSendAsync(new HttpRequestMessage(HttpMethod.Get, "http://localhost"),
                CancellationToken.None);

            Assert.That(response, Is.Null);
        }

        [Test]
        public void Verify_WithMatchingRequests_ChecksOccurrences()
        {
            _handler.Requests.Add(new HttpRequestMessage(HttpMethod.Get, "http://localhost/1"));
            _handler.Requests.Add(new HttpRequestMessage(HttpMethod.Get, "http://localhost/2"));

            bool occurrencesChecked = false;

            _handler.Verify(request => request.RequestUri.AbsoluteUri.Equals("http://localhost/1"),
                occurrences =>
                {
                    occurrencesChecked = true;
                    return occurrences.Equals(1);
                });

            Assert.That(occurrencesChecked, Is.True);
        }

        [Test]
        public void Verify_NoMatchingRequests_ChecksOccurrencesAndThrowsExpectationException()
        {
            _handler.Requests.Add(new HttpRequestMessage(HttpMethod.Get, "http://localhost/1"));
            _handler.Requests.Add(new HttpRequestMessage(HttpMethod.Get, "http://localhost/2"));

            bool occurrencesChecked = false;

            Assert.Throws<ExpectationException>(() => _handler.Verify(
                request => request.RequestUri.AbsoluteUri.Equals("http://localhost/1"),
                occurrences =>
                {
                    occurrencesChecked = true;
                    return occurrences.Equals(2);
                }));

            Assert.That(occurrencesChecked, Is.True);
        }

        private class HandlerTestTarget : MockHttpMessageHandler
        {
            public Task<HttpResponseMessage> TestSendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}
