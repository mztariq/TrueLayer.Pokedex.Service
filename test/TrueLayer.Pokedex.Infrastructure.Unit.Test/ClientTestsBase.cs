using TrueLayer.Pokedex.Domain.Models;
using TrueLayer.Pokedex.Domain.Models.Translation;

namespace TrueLayer.Pokedex.Infrastructure.Unit.Test
{
    public class ClientTestsBase
    {
        public static PokemonDto CreateDummyApiResponse()
        {
            return new PokemonDto
            {
                Name = "ditto",
                FlavorTextEntries = new[]
                {
                    new PokemonFlavorText
                    {
                        FlavorText = "It can freely recombine its own cellular structure to\ntransform into other life-forms.",
                        Language = new PokemonFlavorTextEntryLanguage
                        {
                            Name = "en"
                        }
                    }
                },
                Habitat = new PokemonHabitat
                {
                    Name = "urban"
                },
                IsLegendary = false
            };
        }


        public static Translations CreateDummyTrsnslationResponse()
        {
            return new Translations
            {
                Contents = new TranslationsContents { Translated = "'t can freely recombine its own cellular structure to transform into other life-forms."},
                Success = new TranslationsSuccess { Total = 1 }
            };
        }
    }
}
