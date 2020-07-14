using System;
using System.Net.Http;

namespace DevOceans.Extensions.Testing
{
    public static class MockHttpClientExtensions
    {
        public static Expectation Setup(this MockHttpClient client, Func<HttpRequestMessage, bool> expression)
        {
            return client.MockHttpMessageHandler.Setup(expression);
        }

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