namespace SamplePollyRefit.Configuration
{
    public class HeroApiConfiguration
    {
        public string ApiUrlBase { get; set; }
        public int RetryCount { get; set; }
        public int RetrySleepMilliseconds { get; set; }
        public int TimeoutMillisegundos { get; set; }
    }
}
