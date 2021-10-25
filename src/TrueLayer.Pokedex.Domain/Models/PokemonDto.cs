using System.Text.Json.Serialization;

namespace TrueLayer.Pokedex.Domain.Models
{
    public class PokemonDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("habitat")]
        public PokemonHabitat Habitat { get; set; }

        [JsonPropertyName("is_legendary")]
        public bool IsLegendary { get; set; }

        [JsonPropertyName("flavor_text_entries")]
        public PokemonFlavorText[] FlavorTextEntries { get; set; }
    }
}