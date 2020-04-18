using System.Threading.Tasks;
using RecipesBookDomain.Models;

namespace RecipesBookDomain.Repositories
{
    public interface IRecipeRepository
    {
         Task<Recipe> CreateRecipe(Recipe recipe);
         Task<Recipe> GetRecipe(int id);
         Task<Recipe> UpdateRecipe(int id, RecipeUpdateModel recipeUpdateModel);
         Task DeleteRecipe(int id);
         Task<bool> Exists(int id);
    }
}