using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Enums;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Domain.Models.Translation;
using TrueLayer.Pokedex.Infrastructure.HttpClients;
using Xunit;

namespace TrueLayer.Pokedex.Infrastructure.Unit.Test
{
    public class TranslationHttpClientTests : ClientTestsBase
    {
        private readonly Mock<ILogger<TranslationHttpClient>> _mockLogger = new();

        [Fact]
        public async void SendRequestToTranslationApiReturnsExpectedResponseWith200OK()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            var expected = CreateDummyTrsnslationResponse();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""success"": {""total"": 1 }, ""contents"": { ""translated"": ""'t can freely recombine its own cellular structure to transform into other life-forms."",""text"": ""It can freely recombine its own cellular structure to transform into other life-forms."",""translation"": ""shakespeare""}}")
            };



            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("http://test.com/api/v2/pokemon"),
            };

            var _sut = new TranslationHttpClient(_mockLogger.Object, httpClient);

            var result = await _sut.GetTranslatedData<Translations>("It can freely recombine its own cellular structure to transform into other life-forms.", TranslatorType.Shakespeare);

            Assert.True(expected.Contents.Translated == result.Contents.Translated);
        }

    }
}
