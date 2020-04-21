using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipesBookDal.Extensions;
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

            if(recipeUpdateModel.IngridientIds != null && recipeUpdateModel.IngridientIds.Count() != 0)
            {
                recipe.IngridientsIds = recipeUpdateModel.IngridientIds;
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

        public async Task<List<Recipe>> SearchRecipes(SearchRecipeModel searchRecipeModel)
        {
            var query = _applicationContext.Recipes.AsQueryable();

            if(searchRecipeModel.Name != null)
            {
                query = query.Where(r => r.Name.Contains(searchRecipeModel.Name));
            }
            
            if(searchRecipeModel.LowTime.HasValue)
            {
                query = query.Where(r => r.Time <= searchRecipeModel.LowTime);
            }

            if(searchRecipeModel.HighTime.HasValue)
            {
                query = query.Where(r => r.Time >= searchRecipeModel.HighTime);
            }

            if(searchRecipeModel.LowTotalCost.HasValue)
            {
                query = query.Where(r => r.TotalCost <= searchRecipeModel.LowTotalCost);
            }

            if(searchRecipeModel.HighTotalCost.HasValue)
            {
                query = query.Where(r => r.TotalCost >= searchRecipeModel.HighTotalCost);
            }

            if(searchRecipeModel.IngridientsIds != null && searchRecipeModel.IngridientsIds.Count != 0)
            {
                query = query.Where(r => r.RecipeIngridients.Select(ri => ri.IngridientId).Any(id => searchRecipeModel.IngridientsIds.Contains(id)));
            }

            return await query.Select(r => r.WithIngridientsIds(r.RecipeIngridients.Select(ri => ri.IngridientId))).ToListAsync();
        }
    }
}