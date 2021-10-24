using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public interface IPokemonHttpClient
    {
        Task<T?> GetEntityData<T>(string name);
    }
}