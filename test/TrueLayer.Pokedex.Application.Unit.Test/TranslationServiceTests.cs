using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Enums;
using TrueLayer.Pokedex.Domain.Models.Translation;
using TrueLayer.Pokedex.Infrastructure.HttpClients;
using Xunit;

namespace TrueLayer.Pokedex.Application.Unit.Test
{
    public class TranslationServiceTests : ServiceTestsBase
    {
        private readonly Mock<ITranslationHttpClient> _mockTranslationHttpClient = new();
        private readonly Mock<ILogger<TranslationService>> _mockLogger = new();
        private readonly TranslationService _sut;

        public TranslationServiceTests()
        {
            _sut = new TranslationService(_mockTranslationHttpClient.Object, _mockLogger.Object);
        }


        [Theory]
        [InlineData(TranslatorType.Shakespeare)]
        [InlineData(TranslatorType.Yoda)]
        public async void PassingCorrectPokemonNameToGetTranslationShouldReturnPokemonDetailObject(TranslatorType type)
        {
            var expected = GetTranslation();
            var description = "Test Description of ditto";
            _mockTranslationHttpClient.Setup(x => x.GetTranslatedData<Translations>(description, type)).ReturnsAsync(expected);

            var result = await _sut.Translate(GetPokemonDetail("ditto"));

            Assert.IsType<string>(result);
        }

    }
}
