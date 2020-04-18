using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesBookDomain.Models
{
    public class Ingridient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Kcal { get; set; }
        public ICollection<RecipeIngridient> RecipeIngridients { get; set; } = new List<RecipeIngridient>();
        [NotMapped]
        public List<int> RecipeIds { get; set; } = new List<int>();

    }
}