using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RecipesBookDomain.Models;
using RecipesBookDomain.Services;
using RecipesBookWebApi.Filters;
using RecipesBookWebApi.Models;

namespace RecipesBookWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IRecipeService _recipeService;
        private readonly IMapper _mapper;

        public RecipeController(IRecipeService recipeService, ILogger<RecipeController> logger, IMapper mapper)
        {
            _logger = logger;
            _recipeService = recipeService;
            _mapper = mapper;
        }
        [ExceptionFilterAttribute]
        [HttpPost]
        public async Task<RecipeApiModel> CreateRecipe(Recipe recipe)
        {
            return _mapper.Map<RecipeApiModel>(await _recipeService.CreateRecipe(_mapper.Map<Recipe>(recipe)));
        }

        [ExceptionFilterAttribute]
        [HttpPut("{id:int}")]
        public async Task<RecipeUpdateApiModel> UpdateRecipe(int id, [FromBody]RecipeUpdateApiModel updateRecipeApiModel)
        {
            return _mapper.Map<RecipeUpdateApiModel>(await _recipeService.UpdateRecipe(id, _mapper.Map<RecipeUpdateModel>(updateRecipeApiModel)));
        }

        [ExceptionFilterAttribute]
        [HttpGet("{id:int}")]
        public async Task<RecipeApiModel> GetRecipe(int id)
        {
            return _mapper.Map<RecipeApiModel>(await _recipeService.GetRecipe(id));
        }

        [ExceptionFilterAttribute]
        [HttpDelete("{id:int}")]
        public async Task DeleteRecipe(int id)
        {
            await _recipeService.DeleteRecipe(id);
        }

        [ExceptionFilterAttribute]
        [HttpPost("search")]
        public async Task<List<RecipeApiModel>> SearchRecipes(SearchRecipeApiModel searchRecipeApiModel)
        {
            return _mapper.Map<List<RecipeApiModel>>(await _recipeService.SearchRecipes(_mapper.Map<SearchRecipeModel>(searchRecipeApiModel)));
        }

    }
}
