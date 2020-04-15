using RecipesBookDomain.Models;

namespace RecipesBookDomain.Repositories
{
    public interface IRecipeRepository
    {
         Recipe CreateRecipe(Recipe recipe);
    }
}