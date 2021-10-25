using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Infrastructure.HttpClients;
using Xunit;

namespace TrueLayer.Pokedex.Infrastructure.Unit.Test
{
    public class PokemonHttpClientTests : ClientTestsBase
    {
        private readonly Mock<ILogger<PokemonHttpClient>> _mockLogger = new();

        [Fact]
        public async void SendRequestToPokemonApiReturnsExpectedResponseWith200OK()
        {
            var name = "ditto";
            var handlerMock = new Mock<HttpMessageHandler>();
            var expected = CreateDummyApiResponse();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""base_happiness"":70,""capture_rate"":35,""color"":{""name"":""purple"",""url"":""https://pokeapi.co/api/v2/pokemon-color/7/""},""egg_groups"":[{""name"":""ditto"",""url"":""https://pokeapi.co/api/v2/egg-group/13/""}],""evolution_chain"":{""url"":""https://pokeapi.co/api/v2/evolution-chain/66/""},""evolves_from_species"":null,""flavor_text_entries"":[{""flavor_text"":""It can freely recombine its own cellular structure to\ntransform into other life-forms."",""language"":{""name"":""en"",""url"":""https://pokeapi.co/api/v2/language/9/""},""version"":{""name"":""y"",""url"":""https://pokeapi.co/api/v2/version/24/""}}],""form_descriptions"":[],""forms_switchable"":false,""gender_rate"":-1,""genera"":[{""genus"":""へんしんポケモン"",""language"":{""name"":""ja-Hrkt"",""url"":""https://pokeapi.co/api/v2/language/1/""}},{""genus"":""변신포켓몬"",""language"":{""name"":""ko"",""url"":""https://pokeapi.co/api/v2/language/3/""}},{""genus"":""變身寶可夢"",""language"":{""name"":""zh-Hant"",""url"":""https://pokeapi.co/api/v2/language/4/""}},{""genus"":""Pokémon Morphing"",""language"":{""name"":""fr"",""url"":""https://pokeapi.co/api/v2/language/5/""}},{""genus"":""Transform"",""language"":{""name"":""de"",""url"":""https://pokeapi.co/api/v2/language/6/""}},{""genus"":""Pokémon Transform."",""language"":{""name"":""es"",""url"":""https://pokeapi.co/api/v2/language/7/""}},{""genus"":""Pokémon Mutante"",""language"":{""name"":""it"",""url"":""https://pokeapi.co/api/v2/language/8/""}},{""genus"":""Transform Pokémon"",""language"":{""name"":""en"",""url"":""https://pokeapi.co/api/v2/language/9/""}},{""genus"":""へんしんポケモン"",""language"":{""name"":""ja"",""url"":""https://pokeapi.co/api/v2/language/11/""}},{""genus"":""变身宝可梦"",""language"":{""name"":""zh-Hans"",""url"":""https://pokeapi.co/api/v2/language/12/""}}],""generation"":{""name"":""generation-i"",""url"":""https://pokeapi.co/api/v2/generation/1/""},""growth_rate"":{""name"":""medium"",""url"":""https://pokeapi.co/api/v2/growth-rate/2/""},""habitat"":{""name"":""urban"",""url"":""https://pokeapi.co/api/v2/pokemon-habitat/8/""},""has_gender_differences"":false,""hatch_counter"":20,""id"":132,""is_baby"":false,""is_legendary"":false,""is_mythical"":false,""name"":""ditto"",""names"":[{""language"":{""name"":""ja-Hrkt"",""url"":""https://pokeapi.co/api/v2/language/1/""},""name"":""メタモン""},{""language"":{""name"":""roomaji"",""url"":""https://pokeapi.co/api/v2/language/2/""},""name"":""Metamon""},{""language"":{""name"":""ko"",""url"":""https://pokeapi.co/api/v2/language/3/""},""name"":""메타몽""},{""language"":{""name"":""zh-Hant"",""url"":""https://pokeapi.co/api/v2/language/4/""},""name"":""百變怪""},{""language"":{""name"":""fr"",""url"":""https://pokeapi.co/api/v2/language/5/""},""name"":""Métamorph""},{""language"":{""name"":""de"",""url"":""https://pokeapi.co/api/v2/language/6/""},""name"":""Ditto""},{""language"":{""name"":""es"",""url"":""https://pokeapi.co/api/v2/language/7/""},""name"":""Ditto""},{""language"":{""name"":""it"",""url"":""https://pokeapi.co/api/v2/language/8/""},""name"":""Ditto""},{""language"":{""name"":""en"",""url"":""https://pokeapi.co/api/v2/language/9/""},""name"":""Ditto""},{""language"":{""name"":""ja"",""url"":""https://pokeapi.co/api/v2/language/11/""},""name"":""メタモン""},{""language"":{""name"":""zh-Hans"",""url"":""https://pokeapi.co/api/v2/language/12/""},""name"":""百变怪""}],""order"":156,""pal_park_encounters"":[{""area"":{""name"":""field"",""url"":""https://pokeapi.co/api/v2/pal-park-area/2/""},""base_score"":70,""rate"":20}],""pokedex_numbers"":[{""entry_number"":132,""pokedex"":{""name"":""national"",""url"":""https://pokeapi.co/api/v2/pokedex/1/""}},{""entry_number"":132,""pokedex"":{""name"":""kanto"",""url"":""https://pokeapi.co/api/v2/pokedex/2/""}},{""entry_number"":92,""pokedex"":{""name"":""original-johto"",""url"":""https://pokeapi.co/api/v2/pokedex/3/""}},{""entry_number"":92,""pokedex"":{""name"":""updated-johto"",""url"":""https://pokeapi.co/api/v2/pokedex/7/""}},{""entry_number"":261,""pokedex"":{""name"":""updated-unova"",""url"":""https://pokeapi.co/api/v2/pokedex/9/""}},{""entry_number"":138,""pokedex"":{""name"":""kalos-mountain"",""url"":""https://pokeapi.co/api/v2/pokedex/14/""}},{""entry_number"":209,""pokedex"":{""name"":""original-alola"",""url"":""https://pokeapi.co/api/v2/pokedex/16/""}},{""entry_number"":81,""pokedex"":{""name"":""original-ulaula"",""url"":""https://pokeapi.co/api/v2/pokedex/19/""}},{""entry_number"":271,""pokedex"":{""name"":""updated-alola"",""url"":""https://pokeapi.co/api/v2/pokedex/21/""}},{""entry_number"":92,""pokedex"":{""name"":""updated-ulaula"",""url"":""https://pokeapi.co/api/v2/pokedex/24/""}},{""entry_number"":373,""pokedex"":{""name"":""galar"",""url"":""https://pokeapi.co/api/v2/pokedex/27/""}},{""entry_number"":207,""pokedex"":{""name"":""isle-of-armor"",""url"":""https://pokeapi.co/api/v2/pokedex/28/""}},{""entry_number"":132,""pokedex"":{""name"":""updated-kanto"",""url"":""https://pokeapi.co/api/v2/pokedex/26/""}}],""shape"":{""name"":""ball"",""url"":""https://pokeapi.co/api/v2/pokemon-shape/1/""},""varieties"":[{""is_default"":true,""pokemon"":{""name"":""ditto"",""url"":""https://pokeapi.co/api/v2/pokemon/132/""}}]}"),
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

            var _sut = new PokemonHttpClient(_mockLogger.Object, httpClient);

            var result = await _sut.GetEntityData<PokemonDto>(name);

            Assert.True(expected.FlavorTextEntries[0].FlavorText == result.FlavorTextEntries[0].FlavorText);
            Assert.True(expected.FlavorTextEntries[0].Language.Name == result.FlavorTextEntries[0].Language.Name);
            Assert.True(expected.Habitat.Name == result.Habitat.Name);
            Assert.True(expected.IsLegendary == result.IsLegendary);
            Assert.True(expected.Name == result.Name);
        }

    }
}
