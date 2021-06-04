using System;
using System.Net.Http;

namespace DevOceans.Extensions.Testing
{
    public static class MockHttpClientExtensions
    {
        /// <summary>
        /// Sets up an expectation for a <see cref="HttpRequestMessage"/> to be verified.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="expression"></param>
        /// <returns>A <see cref="Expectation"/> to which a return result can be set or an invocation can be verified.</returns>
        /// <remarks>
        /// A <see cref="Expectation"/> defines an expression delegate that will be invoked for any <see cref="HttpRequestMessage"/>.
        /// If the expectation is met (expression returns true):
        /// - the expectation's <see cref="HttpResponseMessage"/> set with <see cref="Expectation.Returns(HttpResponseMessage)" /> or <see cref="Expectation.Returns(Func{HttpRequestMessage, HttpResponseMessage})"/> is returned;
        /// - the expectation is marked to have been met and can be verified.
        ///
        /// Last added expectations are evaluated from last to first, so, for multiple successful expectations, the last one added one will be effective.
        /// </remarks>
        public static Expectation Setup(this MockHttpClient client, Func<HttpRequestMessage, bool> expression)
        {
            return client.MockHttpMessageHandler.Setup(expression);
        }

        /// <summary>
        /// Verifies that 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="expression"></param>
        public static void Verify(this MockHttpClient client, Func<HttpRequestMessage, bool> expression)
        {
            Verify(client, expression, occurrences => occurrences >= 1);
        }

        public static void Verify(this MockHttpClient client, Func<HttpRequestMessage, bool> expression, Func<int, bool> expectedOccurrences)
        {
            client.MockHttpMessageHandler.Verify(expression, expectedOccurrences);
        }

        public static void Verify(this MockHttpClient client, Func<HttpRequestMessage, bool> expression, int expectedOccurrences)
        {
            client.MockHttpMessageHandler.Verify(expression, occurrences => occurrences.Equals(expectedOccurrences));
        }

        public static void VerifyAll(this MockHttpClient client)
        {
            client.MockHttpMessageHandler.VerifyAll();
        }

        public static void Reset(this MockHttpClient client)
        {
            client.MockHttpMessageHandler.Reset();
        }
    }
}