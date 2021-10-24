using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Domain.Models.Translation;

namespace TrueLayer.Pokedex.Application.Unit.Test
{
    public class ServiceTestsBase
    {
        public PokemonDto GetPokemonDto(string name)
        {
            var flavorText = new PokemonFlavorText[]
            {
                new PokemonFlavorText
                {
                    FlavorText = "This is flavor",
                    Language = new PokemonFlavorTextEntryLanguage { Name = "en" }
                }
            };
            var pokemon = new PokemonDto
            {
                FlavorTextEntries = flavorText,
                Habitat = new PokemonHabitat
                {
                    Name = "service"
                },
                IsLegendary = true,
                Name = name
            };

            return pokemon;
        }

        protected static PokemonDetail GetPokemonDetail(string name)
        {
            return new PokemonDetail
            {
                Description = $"Test Description of {name}",
                Habitat = "bogus",
                IsLegendary = true,
                Name = name
            };
        }

        protected static Translations GetTranslation()
        {
            return new Translations
            {
                Contents = new TranslationsContents { Translated = "This is translation" },
                Success = new TranslationsSuccess { Total = 1 }
            };
        }
    }
}
