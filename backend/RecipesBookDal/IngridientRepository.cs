using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipesBookDomain.Models;
using RecipesBookDomain.Repositories;

namespace RecipesBookDal
{
    public class IngridientRepository : IIngridientRepository
    {
        private readonly ApplicationContext _applicationContext;

        public IngridientRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public async Task<Ingridient> CreateIngridient(Ingridient ingridient)
        {
            _applicationContext.Ingridients.Add(ingridient);
            await _applicationContext.SaveChangesAsync();

            return ingridient;
        }
        public async Task<Ingridient> GetIngridient(int id )
        {
            return await _applicationContext.Ingridients.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Ingridient> UpdateIngridient(int id, IngridientUpdateModel ingridientUpdateModel)
        {
            var ingridient = await GetIngridient(id);

            if(ingridientUpdateModel.Name is {})
            {
                ingridient.Name = ingridientUpdateModel.Name;
            }

            if(ingridientUpdateModel.Kcal.HasValue)
            {
                ingridient.Kcal = ingridientUpdateModel.Kcal.Value;
            }
            await _applicationContext.SaveChangesAsync();

            return ingridient;
        }
        public async Task DeleteIngridient(int id)
        {
            _applicationContext.Ingridients.Remove(await GetIngridient(id));
            await _applicationContext.SaveChangesAsync();
        }
        public async Task<bool> Exists(int id)
        {
            return await _applicationContext.Ingridients.AnyAsync(i => i.Id == id);
        }

        public async Task<List<Ingridient>> GetIngridients(IEnumerable<int> ingridientIds)
        {
            return await _applicationContext.Ingridients.Where(i => ingridientIds.Contains(i.Id)).ToListAsync();
        }

        public async Task<List<Ingridient>> SearchIngridients(SearchIngridientModel searchIngridientModel)
        {
            var searchQuery = _applicationContext.Ingridients.AsQueryable();

            if(searchIngridientModel.Name != null)
            {
                searchQuery = searchQuery.Where(i => i.Name.Contains(searchIngridientModel.Name));
            }

            if(searchIngridientModel.LowKcal.HasValue)
            {
                searchQuery = searchQuery.Where(i => i.Kcal >= searchIngridientModel.LowKcal);
            }
            
            if(searchIngridientModel.HighKcal.HasValue)
            {
                searchQuery = searchQuery.Where(i => i.Kcal <= searchIngridientModel.HighKcal);
            }

            return await searchQuery.ToListAsync();
        }
    }
}