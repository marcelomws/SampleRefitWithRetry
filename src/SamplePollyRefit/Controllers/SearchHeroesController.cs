using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SamplePollyRefit.ExternalSevices;
using System;
using System.Threading.Tasks;

namespace SamplePollyRefit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchHeroesController : ControllerBase
    {
        private readonly ISuperHeroesApiService _superHeroesApiService;
        private readonly ILogger<SearchHeroesController> _logger;

        public SearchHeroesController(ISuperHeroesApiService superHeroesApiService, 
            ILogger<SearchHeroesController> logger)
        {
            _superHeroesApiService = superHeroesApiService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _superHeroesApiService.Search("3755405031177007", "batman");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
