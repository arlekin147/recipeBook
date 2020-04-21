using System.Collections.Generic;

namespace RecipesBookDomain.Models
{
    public class SearchRecipeModel
    {
        public string? Name { get; set; }
        public float? LowTime { get; set; }
        public float? HighTime { get; set; }
        public decimal? LowTotalCost { get; set; }
        public decimal? HighTotalCost { get; set; }
        public List<int> IngridientsIds { get; set; }
    }
}