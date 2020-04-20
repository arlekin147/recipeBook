using System;
using System.Linq;
using System.Threading.Tasks;
using RecipesBookBll.Exceptions;
using RecipesBookDomain.Models;
using RecipesBookDomain.Repositories;
using RecipesBookDomain.Services;

namespace RecipesBookBll
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IIngridientService _ingridientService;
        public RecipeService(IRecipeRepository recipeRepository, IIngridientService ingridientService)
        {
            _recipeRepository = recipeRepository;
            _ingridientService = ingridientService;
        }
        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            await _ingridientService.GetIngridients(recipe.IngridientsIds); //Throws exception when one or more ingridients don't exist

            if (string.IsNullOrEmpty(recipe.Name))
            {
                throw new EntityException("Recipe's name can't be null or empty");
            }

            if (recipe.Time <= 0)
            {
                throw new EntityException("Time can't be null or negative");
            }

            if(recipe.TotalCost <= 0)
            {
                throw new EntityException("Total cost can't be null or negative");
            }

            return await _recipeRepository.CreateRecipe(recipe);
        }
        public async Task<Recipe> GetRecipe(int id)
        {
            await CheckExisting(id);

            return await _recipeRepository.GetRecipe(id);
        }
        public async Task<Recipe> UpdateRecipe(int id, RecipeUpdateModel recipeUpdateModel)
        {
            await CheckExisting(id);

            await _ingridientService.GetIngridients(recipeUpdateModel.IngridientIds); //Checking that each ingridient exists

            if ("".Equals(recipeUpdateModel.Name))
            {
                throw new EntityException("Recipe's name can't be empty");
            }

            if (recipeUpdateModel.Time.HasValue && recipeUpdateModel.Time < 0)
            {
                throw new EntityException("Time can't be negative");
            }

            if(recipeUpdateModel.TotalCost.HasValue && recipeUpdateModel.TotalCost < 0)
            {
                throw new EntityException("TotalCost can't be negative");
            }

            return await _recipeRepository.UpdateRecipe(id, recipeUpdateModel);
        }
        public async Task DeleteRecipe(int id)
        {
            await CheckExisting(id);

            await _recipeRepository.DeleteRecipe(id);
        }
        private async Task CheckExisting(int id)
        {
            if (!(await _recipeRepository.Exists(id)))
            {
                throw new EntityDoesNotExistException($"Recipe with {id} doesn't exist");
            }
        }
    }
}
