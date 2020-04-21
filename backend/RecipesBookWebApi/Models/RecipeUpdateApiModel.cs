using System;

namespace RecipesBookWebApi.Models
{
    public class RecipeUpdateApiModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Time { get; set; }
        public decimal? TotalCost { get; set; }
    }
}
