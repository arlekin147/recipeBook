using System;

namespace RecipeBook.Data
{
    public class SearchIngridientsModel
    {
        public string? Name { get; set; }
        public int? LowKcal { get; set; }
        public int? HighKcal { get; set; }
    }
}
