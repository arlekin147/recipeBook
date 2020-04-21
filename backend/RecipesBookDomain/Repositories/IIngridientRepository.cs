using System.Collections.Generic;
using System.Threading.Tasks;
using RecipesBookDomain.Models;

namespace RecipesBookDomain.Repositories
{
    public interface IIngridientRepository
    {
        Task<Ingridient> CreateIngridient(Ingridient ingridient);
        Task<Ingridient> GetIngridient(int id);
        Task<Ingridient> UpdateIngridient(int id, IngridientUpdateModel ingridientUpdateModel);
        Task DeleteIngridient(int id);
        Task<bool> Exists(int id);
        Task<List<Ingridient>> GetIngridients(IEnumerable<int> ingridientIds);
        Task<List<Ingridient>> SearchIngridients(SearchIngridientModel searchIngridientModel);

    }
}