using System.Collections.Generic;

namespace RecipesBookDomain.Models
{
    public class RecipeApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Time { get; set; }
        public decimal TotalCost { get; set; }
        public List<int> IngridientsIds { get; set; } = new List<int>();
    }
}
