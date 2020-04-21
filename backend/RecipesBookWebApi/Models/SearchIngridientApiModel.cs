namespace RecipesBookWebApi.Models
{
    public class SearchIngridientApiModel
    {
        public string? Name { get; set; }
        public int? LowKcal { get; set; }
        public int? HighKcal { get; set; }
    }
}