using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Polly;
using Refit;
using SamplePollyRefit.Configuration;
using SamplePollyRefit.Services.GitHubApi;
using SamplePollyRefit.Services.Handlers;
using Serilog;

namespace SamplePollyRefit
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<HttpClientLogHandler>();
            services.Configure<GitHubApiConfiguration>(Configuration.GetSection("GitHubApi"));

            var apiGitHubConfiguration = Configuration.GetSection("GitHubApi").Get<GitHubApiConfiguration>();

            ConfigureApi<IGitHubApiService>(services, apiGitHubConfiguration);

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SamplePollyRefit", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SamplePollyRefit v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private void ConfigureApi<T>(IServiceCollection services, IApiConfiguration apiConfiguration) where T : class
        {
            var serviceBuilder = services.AddRefitClient<T>(new RefitSettings
            {
                HttpMessageHandlerFactory = () => new HttpClientHandler
                {
                    UseCookies = false,
                    AutomaticDecompression = DecompressionMethods.GZip
                }
            });

            serviceBuilder.ConfigureHttpClient(c => {
                c.BaseAddress = new Uri(apiConfiguration.BaseUrl);
                c.Timeout = TimeSpan.FromMilliseconds(apiConfiguration.TimeoutMillisegundos);
            });

            serviceBuilder.AddHttpMessageHandler<HttpClientLogHandler>();

            serviceBuilder.AddTransientHttpErrorPolicy((builder)
                => builder.WaitAndRetryAsync(
                        apiConfiguration.RetryAttemps,
                        sleepDurationProvider: (i) => TimeSpan.FromMilliseconds(apiConfiguration.RetryAttempsIntervalMilliseconds),
                        onRetry: (result, timeSpan, retryCount, context) => {
                            Log.Error(result.Exception, $"Exception logged: {result.Exception?.GetType().Name}. Attempt: {retryCount}");
                        })
                   );

            //serviceBuilder.AddTransientHttpErrorPolicy((builder)
            //    => builder.CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));

        }
    }
}
