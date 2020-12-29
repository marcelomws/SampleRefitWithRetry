using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.Simmy;
using Polly.Contrib.Simmy.Outcomes;
using Refit;
using SamplePollyRefit.Configuration;
using Serilog;
using System;
using System.Net;
using System.Net.Http;

namespace SamplePollyRefit.Extensions
{
    public static class ExternalServicesExtensions
    {
        public static void AddExternalServiceClient<T>(this IServiceCollection services, IConfiguration configuration, bool enableCaosEnginnering) where T : class
        {
            var apiConfiguration = configuration.GetSection("HeroApiConfiguration").Get<HeroApiConfiguration>();

            var apiUrlBase = configuration.GetValue<string>("ApiUrlBase");

            var serviceBuilder = services.AddRefitClient<T>(new RefitSettings {
                HttpMessageHandlerFactory = () => new HttpClientHandler
                {
                    UseCookies = false,
                    AutomaticDecompression = DecompressionMethods.GZip
                }
            });

            serviceBuilder.ConfigureHttpClient(c => {
                c.BaseAddress = new Uri(apiConfiguration.ApiUrlBase);
                c.Timeout = TimeSpan.FromMilliseconds(apiConfiguration.TimeoutMillisegundos);
            });

            serviceBuilder.AddHttpMessageHandler<HttpClientLogHandler>();

            serviceBuilder.AddTransientHttpErrorPolicy((builder) 
                => builder.WaitAndRetryAsync(
                        apiConfiguration.RetryCount,
                        sleepDurationProvider: (i) => TimeSpan.FromMilliseconds(apiConfiguration.RetrySleepMilliseconds),
                        onRetry: (result, timeSpan, retryCount, context) => { 
                            Log.Error(result.Exception, $"Exception logged: {result.Exception?.GetType().Name}. Attempt: {retryCount}"); 
                        })
                   );

            if(enableCaosEnginnering) AddChaosEngineering(serviceBuilder);
        }

        private static void AddChaosEngineering(IHttpClientBuilder serviceBuilder)
        {
            // Geração de uma mensagem simulado erro HTTP do tipo 500
            var resultInternalServerError = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("Erro gerado em simulação de caos com Simmy...")
            };

            // Criação da Chaos Policy com uma probabilidade de 60% de erro
            var chaosPolicy = MonkeyPolicy
                .InjectResultAsync<HttpResponseMessage>(with =>
                    with.Result(resultInternalServerError)
                        .InjectionRate(0.6)
                        .Enabled(true)
                );

            serviceBuilder.AddPolicyHandler(chaosPolicy);
        }
    }
}
