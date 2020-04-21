using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecipesBookBll.Exceptions;
using RecipesBookDomain.Models;
using RecipesBookDomain.Repositories;
using RecipesBookDomain.Services;

namespace RecipesBookBll
{
    public class IngridientService : IIngridientService
    {
        private readonly IIngridientRepository _ingridientRepository;

        public IngridientService(IIngridientRepository ingridientRepository)
        {
            _ingridientRepository = ingridientRepository;
        }
        
        public async Task<Ingridient> CreateIngridient(Ingridient ingridient)
        {
            if(ingridient.Kcal < 0) throw new EntityException("Field 'kcal' of ingridient can't be less than 0");
            if(string.IsNullOrEmpty(ingridient.Name)) throw new EntityException("Field 'name' of ingridient can't be null or empty");

            return await _ingridientRepository.CreateIngridient(ingridient);
        }
        public async Task<Ingridient> GetIngridient(int id)
        {
            await CheckExisting(id);

            return await _ingridientRepository.GetIngridient(id);
        }
        public async Task<Ingridient> UpdateIngridient(int id, IngridientUpdateModel ingridientUpdateModel)
        {
            await CheckExisting(id);

            if(ingridientUpdateModel.Kcal.HasValue && ingridientUpdateModel.Kcal < 0) throw new EntityException("Field 'kcal' of ingridient can't be less than 0");
            if("".Equals(ingridientUpdateModel.Name)) throw new EntityException("Field 'name' of ingridient can't be empty");

            return await _ingridientRepository.UpdateIngridient(id, ingridientUpdateModel);
        }
        public async Task DeleteIngridient(int id)
        {
            await CheckExisting(id);

            await _ingridientRepository.DeleteIngridient(id);
        }
        public async Task<List<Ingridient>> GetIngridients(IEnumerable<int> ingridientIds)
        {
            var ingridients = await _ingridientRepository.GetIngridients(ingridientIds);

            if(ingridients.Count != ingridientIds.Count())
            {
                throw new EntityDoesNotExistException("One or more ingridients don't exist");
            }

            return ingridients;
        }
        private async Task CheckExisting(int id)
        {
            if(!(await _ingridientRepository.Exists(id)))
            {
                throw new EntityDoesNotExistException($"Ingridient with id = {id} doesn't exist");
            }
        }

        public async Task<List<Ingridient>> SearchIngridients(SearchIngridientModel searchIngridientModel)
        {
            if(searchIngridientModel != null && "".Equals(searchIngridientModel.Name))
            {
                throw new SearchException("Name for search can't be empty");
            }

            if(searchIngridientModel.LowKcal.HasValue && searchIngridientModel.LowKcal < 0)
            {
                throw new SearchException("The low kcal constraint can't be negative");
            }

            if(searchIngridientModel.HighKcal.HasValue && searchIngridientModel.HighKcal < 0)
            {
                throw new SearchException("The high kcal constraint can't be negative");
            }

            if(searchIngridientModel.LowKcal.HasValue  && 
               searchIngridientModel.HighKcal.HasValue && 
               searchIngridientModel.LowKcal > searchIngridientModel.HighKcal)
            {
                throw new SearchException("The low kcal constraint can't be bigger than high kcal");
            }

            return await _ingridientRepository.SearchIngridients(searchIngridientModel);
        }
    }
}
