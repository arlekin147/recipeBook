using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            //TODO Add transaction
            _applicationContext.Recipes.Add(recipe);
            await _applicationContext.SaveChangesAsync();


            recipe.RecipeIngridients = recipe.IngridientsIds.Select(i => new RecipeIngridient(){ RecipeId = recipe.Id, IngridientId = i }).ToList();
            await _applicationContext.SaveChangesAsync();

            return recipe;
        }
        public async Task<Recipe> GetRecipe(int id)
        {
            return await _applicationContext.Recipes.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Recipe> UpdateRecipe(int id, RecipeUpdateModel recipeUpdateModel)
        {
            var recipe = await GetRecipe(id);

            if(recipeUpdateModel.Name is {})
            {
                recipe.Name = recipeUpdateModel.Name;
            }

            if(recipeUpdateModel.Time.HasValue)
            {
                recipe.Time = recipeUpdateModel.Time.Value;
            }

            if(recipeUpdateModel.TotalCost.HasValue)
            {
                recipe.TotalCost = recipeUpdateModel.TotalCost.Value;
            }

            await _applicationContext.SaveChangesAsync();

            return recipe;
        }
        public async Task DeleteRecipe(int id)
        {
            _applicationContext.Recipes.Remove(await GetRecipe(id));
            await _applicationContext.SaveChangesAsync();
        }
        public async Task<bool> Exists(int id)
        {
            return await _applicationContext.Recipes.AnyAsync(r => r.Id == id);
        }

    }
}