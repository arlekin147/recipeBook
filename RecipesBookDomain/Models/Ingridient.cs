using System.Collections.Generic;

namespace RecipesBookDomain.Models
{
    public class Ingridient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Kcal { get; set; }
        public ICollection<RecipeIngridient> RecipeIngridients { get; set; }
    }
}