using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace DevOceans.Extensions.Testing
{
    public class MockHttpClient : HttpClient
    {
        private readonly MockHttpMessageHandler _messageHandler;

        private MockHttpClient(MockHttpMessageHandler messageHandler) : base(messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public MockHttpMessageHandler MockHttpMessageHandler => _messageHandler;
    }
}
