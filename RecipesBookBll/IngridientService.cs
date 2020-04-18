using System;
using System.Collections.Generic;
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
            return await _ingridientRepository.GetIngridients(ingridientIds);
        }
        private async Task CheckExisting(int id)
        {
            if(!(await _ingridientRepository.Exists(id)))
            {
                throw new EntityDoesNotExistException($"Ingridient with id = {id} doesn't exist");
            }
        }
    }
}
