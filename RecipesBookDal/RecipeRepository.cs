using RecipesBookDomain.Models;
using RecipesBookDomain.Repositories;

namespace RecipesBookDal
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationContext _applicationContext;

        public RecipeRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public Recipe CreateRecipe(Recipe recipe)
        {
            _applicationContext.Recipes.Add(recipe);
            _applicationContext.SaveChanges();

            return recipe;
        }
    }
}