using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public class PokemonHttpClient : IPokemonHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PokemonHttpClient> _logger;

        public PokemonHttpClient(ILogger<PokemonHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<T?> GetEntityData<T>(string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v2/pokemon-species/{name}");

            var response = await SendRequestAsync(request);
            _logger.LogInformation($"Received success code from {_httpClient.BaseAddress}");
            return await DeserializeResponseAsync<T>(response);
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
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