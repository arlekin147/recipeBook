using RecipesBookDomain.Models;

namespace RecipesBookDomain.Services
{
    public interface IRecipeService
    {
        Recipe CreateRecipe(Recipe recipe);
    }
}
