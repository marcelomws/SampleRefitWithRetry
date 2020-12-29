using Refit;
using SamplePollyRefit.ExternalSevices.Response;
using System.Threading.Tasks;

namespace SamplePollyRefit.ExternalSevices
{
    public interface ISuperHeroesApiService
    {
        [Get("/{access_token}/search/{key}")]
        Task<BaseResponse<HeroResultResponse>> Search(string access_token, string key);
    }
}
