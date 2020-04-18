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
    public class IngridientController : ControllerBase
    {
        private readonly ILogger<IngridientController> _logger;
        private readonly IIngridientService _ingridientService;
        private readonly IMapper _mapper;

        public IngridientController(IIngridientService ingridientService, ILogger<IngridientController> logger, IMapper mapper)
        {
            _logger = logger;
            _ingridientService = ingridientService;
            _mapper = mapper;
        }

        [ExceptionFilterAttribute]
        [HttpPost]
        public async Task<IngridientApiModel> CreateIngridient(Ingridient ingridient)
        {
            return _mapper.Map<IngridientApiModel>(await _ingridientService.CreateIngridient(_mapper.Map<Ingridient>(ingridient)));
        }
        [ExceptionFilterAttribute]
        [HttpGet("{id:int}")]
        public async Task<IngridientApiModel> GetIngridient(int id)
        {
            return _mapper.Map<IngridientApiModel>(await _ingridientService.GetIngridient(id));
        }
        [ExceptionFilterAttribute]
        [HttpPut("{id:int}")]
        public async Task<IngridientUpdateApiModel> UpdateIngridient(int id, [FromBody]IngridientUpdateApiModel ingridientUpdateModel)
        {
            return _mapper.Map<IngridientUpdateApiModel>(await _ingridientService.UpdateIngridient(id, _mapper.Map<IngridientUpdateModel>(ingridientUpdateModel)));
        }
        [ExceptionFilterAttribute]
        [HttpDelete("{id:int}")]
        public async Task DeleteIngridient(int id)
        {
            await _ingridientService.DeleteIngridient(id);
        }

    }
}
