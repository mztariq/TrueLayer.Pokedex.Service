using System.Text.Json.Serialization;

namespace TrueLayer.Pokedex.Domain.Models
{
    public class PokemonFlavorText
    {
        [JsonPropertyName("flavor_text")]
        public string FlavorText { get; set; }

        [JsonPropertyName("language")]
        public PokemonFlavorTextEntryLanguage Language { get; set; }
    }
}