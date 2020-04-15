using System;
using System.Collections.Generic;

namespace RecipesBookDomain.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Time { get; set; }
        public decimal TotalCost { get; set; }
        public ICollection<RecipeIngridient> RecipeIngridients { get; set; }
    }
}
