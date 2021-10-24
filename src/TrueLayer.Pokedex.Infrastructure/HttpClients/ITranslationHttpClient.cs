using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Enums;
using TrueLayer.Pokedex.Domain.Models;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public interface ITranslationHttpClient
    {
        Task<T> GetTranslatedData<T>(string description, TranslatorType translatorType);
    }
}