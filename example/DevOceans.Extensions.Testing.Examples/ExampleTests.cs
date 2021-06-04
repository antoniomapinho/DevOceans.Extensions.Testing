using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DevOceans.Extensions.Testing.Examples.Clients;
using DevOceans.Extensions.Testing.Examples.Contract;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DevOceans.Extensions.Testing.Examples
{
    [TestFixture]
    public class ExampleTests
    {
        private MockHttpClient _httpClientMock;
        private SampleServiceClient _sut;
        private string _baseUri = "http://test/entities";

        [SetUp]
        public void Setup()
        {
            _httpClientMock = MockHttpClient.Create();
            _sut = new SampleServiceClient(_httpClientMock);
        }

        [Test]
        public async Task GetById_SetupWithDelegate_SuccessResponse_ReturnsExpectedEntity()
        {
            var id = Guid.NewGuid().ToString();

            var uri = $"{_baseUri}/{id}";

            _httpClientMock.Setup(
                    request =>
                        request.Method.Equals(HttpMethod.Get) && request.RequestUri.AbsoluteUri.Equals(uri))
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new SampleEntity
                        {
                            Id = id,
                            Name = "Sample Entity"
                        }),
                        Encoding.UTF8,
                        "application/json")
                });

            var result = await _sut.GetById(id);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public async Task GetById_SetupWithTypedPayload_SuccessResponse_ReturnsExpectedEntity()
        {
            var id = Guid.NewGuid().ToString();

            var uri = $"{_baseUri}/{id}";

            _httpClientMock.Setup(
                    request =>
                        request.Method.Equals(HttpMethod.Get) && request.RequestUri.AbsoluteUri.Equals(uri))
                .Returns(
                    System.Net.HttpStatusCode.OK,
                    new SampleEntity
                    {
                        Id = id,
                        Name = "Sample Entity"
                    },
                    "application/json");

            var result = await _sut.GetById(id);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(id, result.Id);
        }

        [Test]
        public async Task Create_SuccessResponse_ReturnsExpectedEntity()
        {
            var id = Guid.NewGuid().ToString();

            var entity = new SampleEntity
            {
                Id = id,
                Name = "Sample Entity"
            };

            var uri = $"{_baseUri}";

            _httpClientMock.Setup(request => 
                    request.Method.Equals(HttpMethod.Post) 
                    && request.RequestUri.AbsoluteUri.Equals(uri)
                    && request.ForType<SampleEntity>(e => e.Id.Equals(id))
                    )
                .Returns(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(entity),
                        Encoding.UTF8,
                        "application/json")
                });

            var result = await _sut.Create(entity);

            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(id, result.Id);
        }
    }
}