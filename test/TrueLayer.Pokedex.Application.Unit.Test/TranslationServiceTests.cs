using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Enums;
using TrueLayer.Pokedex.Domain.Models;
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
        [InlineData("this is description", TranslatorType.Shakespeare)]        
        [InlineData("description of pokemon", TranslatorType.Yoda)]
        public async void PassingCorrectPokemonNameToGetTranslationShouldReturnPokemonDetailObject(string description, TranslatorType type)
        {
            _mockTranslationHttpClient.Setup(x => x.GetTranslatedData<Translations>(description, type)).ReturnsAsync(GetTranslation());
            object p = _mockTranslationHttpClient.Setup(x => x.GetTranslatedData<Translations>(description, type)).ReturnsAsync(GetTranslation());



            var result = await _sut.Translate(GetPokemonDetail(description));

            Assert.IsType<string>(result);
        }


        [Theory]
        [InlineData("iamwrong", TranslatorType.Shakespeare)]        
        [InlineData("pleasehelp", TranslatorType.Yoda)]
        public async void PassingInvalidPokemonNameToGetTranslationShouldThrowException(string name, TranslatorType type)
        {
            _mockTranslationHttpClient.Setup(x => x.GetTranslatedData<Translations>(name, type)).Throws<HttpRequestException>();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.Translate(GetPokemonDetail(name)));
        }
    }
}
