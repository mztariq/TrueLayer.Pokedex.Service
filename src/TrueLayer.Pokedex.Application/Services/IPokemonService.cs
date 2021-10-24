using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Application.Services
{
    public interface IPokemonService
    {
        Task<PokemonDetail> GetPokemonDetail(string name);
        Task<PokemonDetail> GetPokemonTranslatedDetail(string name);
    }
}