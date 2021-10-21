using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Application.Services
{
    public class SampleService : ISampleService
    {
        public Task Process(SomeModelData modelRequest)
        {
            return Task.CompletedTask;
        }
    }
}