using Newtonsoft.Json;

namespace SamplePollyRefit.ExternalSevices.Response
{
    public class AppearanceResponse
    {
        public string Gender { get; set; }
        public string Race { get; set; }
        public string[] Height { get; set; } = new string[0];
        public string[] Weight { get; set; } = new string[0];
        [JsonProperty("eye-color")]
        public string EyeColor { get; set; }
        [JsonProperty("hair-color")]
        public string HairColor { get; set; }
    }
}
