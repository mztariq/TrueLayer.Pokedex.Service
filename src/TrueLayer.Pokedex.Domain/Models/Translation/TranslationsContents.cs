using System.Text.Json.Serialization;

namespace TrueLayer.Pokedex.Domain.Models.Translation
{
    public class TranslationsContents
    {
        [JsonPropertyName("translated")]
        public string Translated { get; set; }
    }
}