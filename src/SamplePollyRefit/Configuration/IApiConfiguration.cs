namespace SamplePollyRefit.Configuration
{
    public interface IApiConfiguration
    {
        string BaseUrl { get; set; }
        int RetryAttemps { get; set; }
        int RetryAttempsIntervalMilliseconds { get; set; }
        int TimeoutMillisegundos { get; set; }
    }
}
