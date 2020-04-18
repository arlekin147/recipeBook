using System.Runtime.Serialization;

namespace RecipesBookDomain.Models
{
    public class RecipeIngridient
    {
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public int IngridientId { get; set; }
        public Ingridient Ingridient { get; set; }
    }
}