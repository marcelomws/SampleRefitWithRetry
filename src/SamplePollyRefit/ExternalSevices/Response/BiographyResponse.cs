using Newtonsoft.Json;

namespace SamplePollyRefit.ExternalSevices.Response
{
    public class BiographyResponse
    {
        [JsonProperty("full-name")]
        public string FullName { get; set; }
        [JsonProperty("alter-egos")]
        public string AlterEgos { get; set; }
        public string[] Aliases { get; set; } = new string[0];
        [JsonProperty("place-of-birth")]
        public string PlaceOfBirth { get; set; }
        [JsonProperty("first-appearance")]
        public string FirstAppearance { get; set; }
        public string Publisher { get; set; }
        public string Alignment { get; set; }
    }
}
