using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesBookDomain.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Time { get; set; }
        public decimal TotalCost { get; set; }
        
        [NotMapped]
        public List<int> IngridientsIds { get; set; } = new List<int>();
        public ICollection<RecipeIngridient> RecipeIngridients { get; set; } = new List<RecipeIngridient>();
    }
}
