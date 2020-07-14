using System;
using System.Net.Http;

namespace DevOceans.Extensions.Testing
{
    public class ExpectationException : Exception
    {

        public ExpectationException(string message) : base(message)
        {
        }

        public ExpectationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExpectationException(string message, HttpRequestMessage request) : base(message)
        {
            Request = request;
        }

        public ExpectationException(string message, Expectation expectation) : base(message)
        {
            Expectation = expectation;
        }

        public HttpRequestMessage Request { get; }

        public Expectation Expectation { get; }
    }
}