using Microsoft.Extensions.Logging;
using Moq;
using System.Net.Http;
using TrueLayer.Pokedex.Application.Services;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Infrastructure.HttpClients;
using Xunit;

namespace TrueLayer.Pokedex.Application.Unit.Test
{
    public class PokemonServiceTests : ServiceTestsBase
    {
        private readonly Mock<IPokemonHttpClient> _mockPokemonHttpClient = new();
        private readonly Mock<ITranslationService> _mockTranslationService = new();
        private readonly Mock<ILogger<PokemonService>> _mockLogger = new();
        private readonly PokemonService _sut;

        public PokemonServiceTests()
        {
            _sut = new PokemonService(_mockPokemonHttpClient.Object, _mockLogger.Object, _mockTranslationService.Object);
        }

        [Theory]
        [InlineData("pokemon")]        
        [InlineData("pokemon2")]
        public async void PassingCorrectPokemonNameToGetPokemonDetailShouldReturnPokemonDetailObject(string name)
        {
            _mockPokemonHttpClient.Setup(x => x.GetEntityData<PokemonDto>(name)).ReturnsAsync(GetPokemonDto(name));

            var result = await _sut.GetPokemonDetail(name);

            Assert.Equal(name, result.Name);
            Assert.IsType<PokemonDetail>(result);
        }


        [Theory]
        [InlineData("iamwrong")]        
        [InlineData("pleasehelp")]
        public async void PassingInvalidPokemonNameToGetPokemonDetailShouldThrowException(string name)
        {
            _mockPokemonHttpClient.Setup(x => x.GetEntityData<PokemonDto>(name)).Throws<HttpRequestException>();

            await Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.GetPokemonDetail(name));
        }
    }
}
