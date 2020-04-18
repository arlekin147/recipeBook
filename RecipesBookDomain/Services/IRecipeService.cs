using System.Threading.Tasks;
using RecipesBookDomain.Models;

namespace RecipesBookDomain.Services
{
    public interface IRecipeService
    {
        Task<Recipe> CreateRecipe(Recipe recipe);
        Task<Recipe> GetRecipe(int id);
        Task<Recipe> UpdateRecipe(int id, RecipeUpdateModel recipeUpdateModel);
        Task DeleteRecipe(int id);
    }
}
