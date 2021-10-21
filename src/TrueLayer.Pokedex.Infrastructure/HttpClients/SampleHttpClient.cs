using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public class SampleHttpClient : ISampleHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<SampleHttpClient> _logger;

        public SampleHttpClient(ILogger<SampleHttpClient> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<T?> GetEntityData<T>(string somevariableOrCanBeModelInsteadOfString)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"?sample={somevariableOrCanBeModelInsteadOfString}");

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
            var result = JsonSerializer.Deserialize<IEnumerable<T>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result is not null 
                ? result.FirstOrDefault() 
                : default;
        }
    }
}