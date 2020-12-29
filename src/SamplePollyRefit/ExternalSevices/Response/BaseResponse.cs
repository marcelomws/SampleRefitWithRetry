using Newtonsoft.Json;

namespace SamplePollyRefit.ExternalSevices.Response
{
    public class BaseResponse<TResult>
    {
        public string Response { get; set; }

        [JsonProperty("results-for")]
        public string ResultsFor { get; set; }

        public TResult[] Results { get; set; } = new TResult[0];
    }
}
