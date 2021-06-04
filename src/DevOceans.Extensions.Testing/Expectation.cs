using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace DevOceans.Extensions.Testing
{
    public class Expectation
    {
        private bool _verifiable;

        public Expectation(Func<HttpRequestMessage, bool> requestExpression)
        {
            RequestExpression = requestExpression;
        }

        public Expectation(Func<HttpRequestMessage, bool> requestExpression, Func<HttpRequestMessage, HttpResponseMessage> responseExpression)
        {
            RequestExpression = requestExpression;
            ResponseExpression = responseExpression;
        }

        public Func<HttpRequestMessage, bool> RequestExpression { get; }

        public Func<HttpRequestMessage, HttpResponseMessage> ResponseExpression { get; private set;}
        
        public bool Invoked { get; set; }

        public Expectation Returns(Func<HttpRequestMessage, HttpResponseMessage> responseExpression)
        {
            ResponseExpression = responseExpression;

            return this;
        }

        public Expectation Returns(HttpResponseMessage responseExpression)
        {
            ResponseExpression = request => responseExpression;

            return this;
        }

        public Expectation Returns<T>(HttpStatusCode statusCode, T payload, string mediaType)
        {
            return Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(payload),
                    Encoding.UTF8,
                    mediaType)
            });
        }

        public void Verifiable()
        {
            _verifiable = true;
        }

        public void Verify()
        {
            var verified = !_verifiable || (_verifiable && Invoked);

            if (!verified)
            {
                throw new ExpectationException("Verification failed.", this);
            }
        }
    }

}