using System;
using RecipesBookDomain.Models;
using RecipesBookDomain.Repositories;
using RecipesBookDomain.Services;

namespace RecipesBookBll
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public Recipe CreateRecipe(Recipe recipe)
        {
            return _recipeRepository.CreateRecipe(recipe);
        }
    }
}
