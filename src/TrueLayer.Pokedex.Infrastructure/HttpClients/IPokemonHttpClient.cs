using System.Threading.Tasks;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public interface IPokemonHttpClient
    {
        Task<T?> GetEntityData<T>(string name);
    }
}