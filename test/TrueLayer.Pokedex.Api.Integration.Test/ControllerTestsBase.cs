using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Api.Integration.Test
{
    public class ControllerTestsBase : IDisposable
    {
        private static WebApplicationFactory<Startup> _webApplicationFactory;

        protected static PokemonDetail GetPokemonDetail(string name)
        {
            return new PokemonDetail
            {
                Description = $"Test Description of {name}",
                Habitat = "bogus",
                IsLegendary = true,
                Name = name
            };
        }
        protected static async Task<HttpResponseMessage> ExecuteSendRequestAsync(string json,
            string relativeUri)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, relativeUri)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _webApplicationFactory = new WebApplicationFactory<Startup>();
            var response = await _webApplicationFactory.CreateClient().SendAsync(request);

            return response;
        }

        protected static async Task<string> ReadTestData(string relativePath)
        {
            using var streamReader = new StreamReader(relativePath);

            return await streamReader.ReadToEndAsync();
        }

        public void Dispose()
        {
            _webApplicationFactory.Server.Dispose();
            _webApplicationFactory.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
