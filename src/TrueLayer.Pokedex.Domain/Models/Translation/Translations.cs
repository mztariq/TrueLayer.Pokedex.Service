using System.Text.Json.Serialization;

namespace TrueLayer.Pokedex.Domain.Models.Translation
{
    public class Translations
    {
        [JsonPropertyName("success")]
        public TranslationsSuccess Success { get; set; }

        [JsonPropertyName("contents")]
        public TranslationsContents Contents { get; set; }
    }
}