using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DevOceans.Extensions.Testing.Examples.Contract;
using Newtonsoft.Json;

namespace DevOceans.Extensions.Testing.Examples.Clients
{
    public class SampleServiceClient
    {
        private readonly HttpClient _httpClient;

        public SampleServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SampleEntity> GetById(string id)
        {
            var uri = $"http://test/entities/{id}";

            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            return JsonConvert.DeserializeObject<SampleEntity>(
                                await response.Content.ReadAsStringAsync());
                        }
                        catch (JsonException ex)
                        {
                            throw new SerializationException("Unable to deserialize response.", ex);
                        }
                        catch (Exception ex)
                        {
                            throw new RetrievalException($"Unable to retrieve Entity with Id {id}.", ex);
                        }
                    }
                }
            }

            return null;
        }

        public async Task<SampleEntity> Create(SampleEntity entity)
        {
            var uri = "http://test/entities";

            using (var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")
            })
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            return JsonConvert.DeserializeObject<SampleEntity>(
                                await response.Content.ReadAsStringAsync());
                        }
                        catch (JsonException ex)
                        {
                            throw new SerializationException("Unable to deserialize response.", ex);
                        }
                        catch (Exception ex)
                        {
                            throw new CreationException($"Unable to create Entity.", ex);
                        }
                    }
                }
            }

            return null;
        }
    }

    public class SerializationException : Exception
    {
        public SerializationException(string message) : base(message)
        {
        }

        public SerializationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class RetrievalException : Exception
    {
        public RetrievalException(string message) : base(message)
        {
        }

        public RetrievalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class CreationException : Exception
    {
        public CreationException(string message) : base(message)
        {
        }

        public CreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
