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
        

        public async Task<List<Recipe>> SearchRecipes(SearchRecipeModel searchRecipeModel)
        {
            if(searchRecipeModel != null && "".Equals(searchRecipeModel.Name))
            {
                throw new SearchException("Name for search can't be empty");
            }


            if(searchRecipeModel.LowTime.HasValue && searchRecipeModel.LowTime < 0)
            {
                throw new SearchException("The low kcal constraint can't be negative");
            }

            if(searchRecipeModel.HighTime.HasValue && searchRecipeModel.HighTime < 0)
            {
                throw new SearchException("The high kcal constraint can't be negative");
            }

            if(searchRecipeModel.LowTime.HasValue  && 
               searchRecipeModel.LowTime.HasValue && 
               searchRecipeModel.LowTime > searchRecipeModel.HighTime)
            {
                throw new SearchException("The low kcal constraint can't be bigger than high kcal");
            }


            if(searchRecipeModel.LowTotalCost.HasValue && searchRecipeModel.LowTotalCost < 0)
            {
                throw new SearchException("The low kcal constraint can't be negative");
            }

            if(searchRecipeModel.HighTotalCost.HasValue && searchRecipeModel.HighTotalCost < 0)
            {
                throw new SearchException("The high kcal constraint can't be negative");
            }

            if(searchRecipeModel.LowTotalCost.HasValue  && 
               searchRecipeModel.HighTotalCost.HasValue && 
               searchRecipeModel.LowTotalCost > searchRecipeModel.HighTotalCost)
            {
                throw new SearchException("The low kcal constraint can't be bigger than high kcal");
            }

            if(searchRecipeModel.IngridientsIds != null && searchRecipeModel.IngridientsIds.Count != 0)
            {
                try
                {
                    await _ingridientService.GetIngridients(searchRecipeModel.IngridientsIds);
                }
                catch(EntityDoesNotExistException e)
                {
                    throw new SearchException("One or more ingridient from search don't exist", e);
                }
            }

            return await _recipeRepository.SearchRecipes(searchRecipeModel);
        }
    }
}
