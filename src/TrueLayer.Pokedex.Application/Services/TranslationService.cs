using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Enums;
using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Domain.Models.Translation;
using TrueLayer.Pokedex.Infrastructure.HttpClients;

namespace TrueLayer.Pokedex.Application.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly ITranslationHttpClient _translationHttpClient;
        private readonly ILogger<TranslationService> _logger;
        private const string YodaHabitat = "cave";


        public TranslationService(ITranslationHttpClient translationHttpClient, ILogger<TranslationService> logger)
        {
            _translationHttpClient = translationHttpClient;
            _logger = logger;
        }

        public async Task<string> Translate(PokemonDetail pokemon)
        {
            var translatorType = (pokemon.Habitat == YodaHabitat || pokemon.IsLegendary) 
                ? TranslatorType.Yoda : TranslatorType.Shakespeare;

            var result = await _translationHttpClient.GetTranslatedData<Translations>(pokemon.Description, translatorType);
            return result.Contents?.Translated is not null ? result.Contents.Translated : pokemon.Description; 
        }
    }
}