using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace DevOceans.Extensions.Testing
{
    public static class HttpRequestMessageExtensions
    {
        public static Func<HttpRequestMessage, bool> ForType<TMessage>(
            this HttpRequestMessage request, 
            Func<TMessage, bool> typedFunc,
            JsonSerializerSettings jsonSerializerSettings)
        {
            Func<HttpRequestMessage, bool> func = message => 
            {
                var content = message.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                var typed = JsonConvert.DeserializeObject<TMessage>(content, jsonSerializerSettings);

                return typedFunc.Invoke(typed);
            };

            return func;
        }
    }
}