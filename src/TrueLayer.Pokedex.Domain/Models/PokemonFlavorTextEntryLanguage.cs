using System.Text.Json.Serialization;

namespace TrueLayer.Pokedex.Domain.Models
{
    public class PokemonFlavorTextEntryLanguage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}