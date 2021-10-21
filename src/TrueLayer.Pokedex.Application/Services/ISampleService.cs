using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Application.Services
{
    public interface ISampleService
    {
        Task Process(SomeModelData modelRequest);
    }
}