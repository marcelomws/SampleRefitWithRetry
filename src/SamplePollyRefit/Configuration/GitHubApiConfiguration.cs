namespace SamplePollyRefit.Configuration
{
    public class GitHubApiConfiguration : IApiConfiguration
    {
        public string BaseUrl { get; set; }
        public int RetryAttemps { get; set ; }
        public int RetryAttempsIntervalMilliseconds { get; set; }
        public int TimeoutMillisegundos { get; set; }
    }
}
