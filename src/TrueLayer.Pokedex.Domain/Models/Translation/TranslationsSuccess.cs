using System.Text.Json.Serialization;

namespace TrueLayer.Pokedex.Domain.Models.Translation
{
    public class TranslationsSuccess
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}