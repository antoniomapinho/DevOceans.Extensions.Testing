using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace DevOceans.Extensions.Testing
{
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Provides the ability to apply a bool returning function delegate to a typed object in the request content.
        /// </summary>
        /// <typeparam name="TContent">The expected deserialized type of the request Content</typeparam>
        /// <param name="request">The HttpRequestMessage being checked</param>
        /// <param name="contentCheck">The typed content object check function.</param>
        /// <param name="jsonSerializerSettings">An instance of JsonSerializerSettings to use when deserializing the request Content.</param>
        /// <returns>A bool that is the result of the contentCheck function applied to the deserialized content."/></returns>
        /// <remarks>
        /// This method invokes the contentCheck function against the object of type TMessage in the message Content.
        /// It attempts to deserialize the <see cref="HttpRequestMessage"/> Content to TContent before invoking the contentCheck.
        /// </remarks>
        public static bool ForType<TContent>(
            this HttpRequestMessage request, 
            Func<TContent, bool> contentCheck,
            JsonSerializerSettings jsonSerializerSettings)
        {
            var content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var typed = JsonConvert.DeserializeObject<TContent>(content, jsonSerializerSettings);

            return contentCheck.Invoke(typed);
        }

        /// <summary>
        /// Provides the ability to apply a bool returning function delegate to a typed object in the request content.
        /// </summary>
        /// <typeparam name="TContent">The expected deserialized type of the request Content</typeparam>
        /// <param name="request">The HttpRequestMessage being checked</param>
        /// <param name="contentCheck">The typed content object check function.</param>
        /// <returns>A bool that is the result of the contentCheck function applied to the deserialized content."/></returns>
        /// <remarks>
        /// This method invokes the contentCheck function against the object of type TMessage in the message Content.
        /// It attempts to deserialize the <see cref="HttpRequestMessage"/> Content to TContent before invoking the contentCheck.
        /// For custom deserialization, <see cref="ForType{TContent}(HttpRequestMessage, Func{TContent, bool}, JsonSerializerSettings)"/> should be used so a <see cref="JsonSerializerSettings"/> can be passed.
        /// </remarks>
        public static bool ForType<TContent>(
            this HttpRequestMessage request, 
            Func<TContent, bool> contentCheck)
        {
            var content = request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var typed = JsonConvert.DeserializeObject<TContent>(content);

            return contentCheck.Invoke(typed);
        }
    }
}