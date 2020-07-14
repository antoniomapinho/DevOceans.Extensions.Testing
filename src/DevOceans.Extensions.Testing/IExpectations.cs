using System;
using System.Collections.Generic;
using System.Net.Http;

namespace DevOceans.Extensions.Testing
{
    public interface IExpectations
    {
        List<Expectation> Expectations { get; }
        Expectation Setup(Func<HttpRequestMessage, bool> expression);

        void Verify(Func<HttpRequestMessage, bool> expression, int expectedOccurrences);

        void Verify(Func<HttpRequestMessage, bool> expression, Func<int, bool> occurrencesCheck);

        void VerifyAll();

        void Reset();
    }
}