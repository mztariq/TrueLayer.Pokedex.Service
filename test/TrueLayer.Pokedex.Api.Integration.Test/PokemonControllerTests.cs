using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http;
using TrueLayer.Pokedex.Api.Controllers;
using TrueLayer.Pokedex.Application.Services;
using Xunit;

namespace TrueLayer.Pokedex.Api.Integration.Test
{
    public class PokemonControllerTests : ControllerTestsBase
    {
        private readonly string pokemonRelativeUrl = "api/pokemon";
        private readonly string translationRelativeUrl = "api/pokemon/translated";


        [Theory]
        [InlineData("mewtwo")]
        [InlineData("geodude")]
        [InlineData("ditto")]
        public async void WhenPassCorrectPokemonDetailsShouldReturnPokemonDetailsObject(string name)
        {
            var mockPokemonService = new Mock<IPokemonService>();
            var mockLogger = new Mock<ILogger<PokemonController>>();
            var sut = new PokemonController(mockPokemonService.Object, mockLogger.Object);
            mockPokemonService.Setup(x => x.GetPokemonDetail(name)).ReturnsAsync(GetPokemonDetail(name));

            await sut.GetPokemon(name);
            var response = await ExecuteSendRequestAsync(name, $"{pokemonRelativeUrl}/{name}");

            mockPokemonService.Verify(c => c.GetPokemonDetail(It.Is<string>(g => g == name)));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Theory]
        [InlineData("blabla")]
        [InlineData("norealpokemon")]
        [InlineData("iamwrong")]
        public async void WhenPassInvalidPokemonDetailsShouldReturnBadRequest(string name)
        {
            var mockPokemonService = new Mock<IPokemonService>();
            var mockLogger = new Mock<ILogger<PokemonController>>();
            var sut = new PokemonController(mockPokemonService.Object, mockLogger.Object);
            mockPokemonService.Setup(x => x.GetPokemonDetail(name)).Throws<HttpRequestException>();
           

            await sut.GetPokemon(name);
            var response = await ExecuteSendRequestAsync(name, $"{pokemonRelativeUrl}/{name}");

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Theory]
        [InlineData("mewtwo")]
        [InlineData("geodude")]
        [InlineData("ditto")]
        public async void WhenPassCorrectPokemonDescriptionTranslationShouldReturnPokemonDetailsObject(string name)
        {
            var mockPokemonService = new Mock<IPokemonService>();
            var mockLogger = new Mock<ILogger<PokemonController>>();
            var sut = new PokemonController(mockPokemonService.Object, mockLogger.Object);
            mockPokemonService.Setup(x => x.GetPokemonTranslatedDetail(name)).ReturnsAsync(GetPokemonDetail(name));

            await sut.GetTranslatedPokemon(name);
            var response = await ExecuteSendRequestAsync(name, $"{translationRelativeUrl}/{name}");

            mockPokemonService.Verify(c => c.GetPokemonTranslatedDetail(It.Is<string>(g => g == name)));
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Theory]
        [InlineData("blabla")]
        [InlineData("norealpokemon")]
        [InlineData("iamwrong")]
        public async void WhenPassInvalidPokemonDescriptionTranslationShouldReturnBadRequest(string name)
        {
            var mockPokemonService = new Mock<IPokemonService>();
            var mockLogger = new Mock<ILogger<PokemonController>>();
            var sut = new PokemonController(mockPokemonService.Object, mockLogger.Object);
            mockPokemonService.Setup(x => x.GetPokemonTranslatedDetail(name)).Throws<HttpRequestException>();

            await sut.GetTranslatedPokemon(name);
            var response = await ExecuteSendRequestAsync(name, $"{translationRelativeUrl}/{name}");


            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
