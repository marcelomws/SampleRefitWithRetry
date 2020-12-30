using System.Threading.Tasks;
using Refit;
using SamplePollyRefit.Services.GitHubApi.Response;

namespace SamplePollyRefit.Services.GitHubApi
{
    public interface IGitHubApiService
    {
        [Get("/users/{userName}")]
        Task<UserResponse> GetUser(string userName);

        [Get("/users/{userName}/repos")]
        Task<UserResponse[]> GetRepos(string userName);

        [Get("/orgs/{orgName}/members")]
        Task<MemberResponse[]> GetMembers(string orgName);
    }
}
