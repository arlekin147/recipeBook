using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable enable

namespace RecipesBookWebApi.Models
{
    public class IngridientUpdateApiModel
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Kcal { get; set; }
    }
}