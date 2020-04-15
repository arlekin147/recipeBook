using System.Runtime.Serialization;

namespace RecipesBookDomain.Models
{
    public class RecipeIngridient
    {
        public int RecipeId { get; set; }
        
        [IgnoreDataMember]
        public Recipe Recipe { get; set; }

        public int IngridientId { get; set; }

        [IgnoreDataMember]
        public Ingridient Ingridient { get; set; }
    }
}