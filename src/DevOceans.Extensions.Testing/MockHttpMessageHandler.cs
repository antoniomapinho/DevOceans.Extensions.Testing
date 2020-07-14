using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevOceans.Extensions.Testing
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly List<HttpRequestMessage> _requests = new List<HttpRequestMessage>();

        private readonly List<Expectation> _expectations;

        public MockHttpMessageHandler()
        {
            _expectations = new List<Expectation>();
        }
        public List<Expectation> Expectations => _expectations;

        public Expectation Setup(Func<HttpRequestMessage, bool> expression)
        {
            var expectation = new Expectation(expression);

            _expectations.Add(expectation);

            return expectation;
        }

        public void Verify(Func<HttpRequestMessage, bool> expression, int expectedOccurrences)
        {
            Verify(expression, occurrences => occurrences.Equals(expectedOccurrences));
        }

        public void Verify(Func<HttpRequestMessage, bool> expression, Func<int, bool> occurrencesCheck)
        {
            var occurrences = _requests.Count(expression.Invoke);

            var validOccurrences = occurrencesCheck(occurrences);

            if (!validOccurrences)
            {
                throw new ExpectationException(
                    $"Verification mismatch. Expected occurrences verification failed; Actual occurrences: {occurrences}");
            }
        }

        public void VerifyAll()
        {
            var exceptions = new List<ExpectationException>(_expectations.Count);

            foreach (var expectation in _expectations)
            {
                try
                {
                    expectation.Verify();
                }
                catch (ExpectationException ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(
                    "Verification failed for some expectations. Check the inner exceptions.",
                    exceptions);
            }
        }

        public void Reset()
        {
            _expectations.Clear();
            _requests.Clear();
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _requests.Add(request);

            for (var i = _expectations.Count - 1; i >= 0; i--)
            {
                var expectation = _expectations[i];

                if (expectation.RequestExpression.Invoke(request))
                {
                    expectation.Invoked = true;
                    return Task.FromResult(expectation.ResponseExpression.Invoke(request));
                }
            }

            return Task.FromResult<HttpResponseMessage>(null);
        }

        public IEnumerable<HttpRequestMessage> Requests => _requests;
    }
}
