using System.Collections.Generic;

namespace RecipeBook.Data
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Time { get; set; }
        public decimal TotalCost { get; set; }
        public List<int> IngridientsIds { get; set; } = new List<int>();
        public string IngridientsDescription {get; set;} = "";
    }
}