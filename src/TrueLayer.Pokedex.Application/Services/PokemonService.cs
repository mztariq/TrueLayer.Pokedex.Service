using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Infrastructure.HttpClients;

namespace TrueLayer.Pokedex.Application.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonHttpClient _pokemonHttpClient;
        private readonly ITranslationService _translationService;
        private readonly ILogger<PokemonService> _logger;
        private const string Language = "en";


        public PokemonService(IPokemonHttpClient pokemonHttpClient, ILogger<PokemonService> logger, 
            ITranslationService translationService)
        {
            _pokemonHttpClient = pokemonHttpClient;
            _logger = logger;
            _translationService = translationService;
        }

        public async Task<PokemonDetail> GetPokemonDetail(string name)
        {
            _logger.LogInformation("Fetching data from pokemon api");
            var httpResponse = await _pokemonHttpClient.GetEntityData<PokemonDto>(name);
            var result = PokemaonMapper(httpResponse);
            _logger.LogInformation($"Data recieved for {name}");
            return result;
        }

        public async Task<PokemonDetail> GetPokemonTranslatedDetail(string name)
        {
            _logger.LogInformation("Fetching data from pokemon api");
            var pokemon = await GetPokemonDetail(name);
            var result = TranslationMapper(pokemon);
            _logger.LogInformation($"Data recieved and translated for {name}");
            return result;
        }


        private PokemonDetail TranslationMapper(PokemonDetail pokemon)
        {
            var translatedDescription = _translationService.Translate(pokemon).Result;
            return new PokemonDetail
            {
                Name = pokemon.Name,
                Description = translatedDescription,
                Habitat = pokemon.Habitat,
                IsLegendary = pokemon.IsLegendary
            };
        }
        private static PokemonDetail PokemaonMapper(PokemonDto pokemon)
        {
            return new PokemonDetail
            {
                Name = pokemon.Name,
                Description = GetDescription(pokemon.FlavorTextEntries),
                Habitat = pokemon.Habitat?.Name,
                IsLegendary = pokemon.IsLegendary
            };
        }
        private static string GetDescription(PokemonFlavorText[] flavorText)
        {
            return flavorText?
                       .FirstOrDefault(x => Language.Equals(x.Language?.Name, StringComparison.OrdinalIgnoreCase))?
                       .FlavorText
                       .Replace("\n", " ")
                       .Replace("\f", " ")
                   ?? string.Empty;
        }
    }
}