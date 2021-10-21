using System.Threading.Tasks;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public interface ISampleHttpClient
    {
        Task<T?> GetEntityData<T>(string somevariableOrCanBeModelInsteadOfString);
    }
}