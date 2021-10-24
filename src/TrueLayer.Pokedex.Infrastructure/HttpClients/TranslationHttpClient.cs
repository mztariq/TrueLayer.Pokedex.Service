using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Enums;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public class TranslationHttpClient : ITranslationHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TranslationHttpClient> _logger;

        public TranslationHttpClient(ILogger<TranslationHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<T> GetTranslatedData<T>(string description, TranslatorType translatorType)
        {
            var response = await _httpClient
                .PostAsync(
                    $"/translate/{translatorType.ToString().ToLower()}.json",
                    new FormUrlEncodedContent(
                        new[]
                        {
                            new KeyValuePair<string, string>("text", description)
                        }));

            _logger.LogInformation($"Received success code from {_httpClient.BaseAddress}");
            return await DeserializeResponseAsync<T>(response);
        }

        private static async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result is not null
                ? result : default;
        }
    }
}