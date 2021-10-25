using System.Threading.Tasks;
using TrueLayer.Pokedex.Domain.Enums;

namespace TrueLayer.Pokedex.Infrastructure.HttpClients
{
    public interface ITranslationHttpClient
    {
        Task<T> GetTranslatedData<T>(string description, TranslatorType translatorType);
    }
}