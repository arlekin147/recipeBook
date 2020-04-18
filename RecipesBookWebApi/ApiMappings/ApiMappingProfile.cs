using AutoMapper;
using RecipesBookDomain.Models;
using RecipesBookWebApi.Models;

namespace RecipesBookWebApi.ApiMappings
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<IngridientUpdateApiModel, IngridientUpdateModel>().ReverseMap();

            CreateMap<RecipeUpdateApiModel, RecipeUpdateModel>().ReverseMap();

            CreateMap<IngridientApiModel, Ingridient>().ReverseMap();

            CreateMap<RecipeApiModel, Recipe>().ReverseMap();
        }
    }
}