using System.Collections.Generic;
using System.Linq;
using RecipesBookDomain.Models;

namespace RecipesBookDal.Extensions
{
    public static class RecipeModelExtensions
    {
        public static Recipe WithIngridientsIds(this Recipe recipe, IEnumerable<int> ingridientsIds)
        {
            recipe.IngridientsIds = ingridientsIds.ToList();
            return recipe;
        }
    }
}