using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SamplePollyRefit.Services.GitHubApi;

namespace SamplePollyRefit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubApiService _gitHubApiService;

        public GitHubController(IGitHubApiService gitHubApiService)
        {
            _gitHubApiService = gitHubApiService;
        }

        [HttpGet("users/{userName}")]
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var result = await _gitHubApiService.GetUser(userName);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("users/{userName}/repos")]
        public async Task<IActionResult> GetRepos(string userName)
        {
            var result = await _gitHubApiService.GetRepos(userName);
            return Ok(result);
        }

        [HttpGet("orgs/{organization}/repos")]
        public async Task<IActionResult> GetMembers(string organization)
        {
            var result = await _gitHubApiService.GetMembers(organization);
            return Ok(result);
        }
    }
}
