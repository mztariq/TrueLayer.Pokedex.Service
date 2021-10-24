using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Application.Services
{
    public interface ITranslationService
    {
        Task<string> Translate(PokemonDetail pokemon);
    }
}