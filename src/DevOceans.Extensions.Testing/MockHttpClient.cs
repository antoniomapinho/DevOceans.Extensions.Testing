using System.Net.Http;

namespace DevOceans.Extensions.Testing
{
    public class MockHttpClient : HttpClient
    {
        private MockHttpClient(MockHttpMessageHandler messageHandler) : base(messageHandler)
        {
            MockHttpMessageHandler = messageHandler;
        }

        public static MockHttpClient Create()
        {
            return new MockHttpClient(new MockHttpMessageHandler());
        }

        public MockHttpMessageHandler MockHttpMessageHandler { get; }
    }
}
