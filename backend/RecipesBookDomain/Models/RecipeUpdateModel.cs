using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesBookDomain.Models
{
    public class RecipeUpdateModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public float? Time { get; set; }
        public decimal? TotalCost { get; set; }
        public List<int> IngridientIds { get; set; }
    }
}
