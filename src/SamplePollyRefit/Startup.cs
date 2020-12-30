using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SamplePollyRefit.Configuration;
using SamplePollyRefit.Extensions;
using SamplePollyRefit.Services.GitHubApi;
using SamplePollyRefit.Services.Handlers;

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
            services.AddTransient<HttpClientLogHandler>();
            services.Configure<GitHubApiConfiguration>(Configuration.GetSection("GitHubApi"));

            // Passar enableCaosEnginnering = true para habilitar simulação de falhas no retry police
            services.AddExternalServiceClient<IGitHubApiService>(Configuration, enableCaosEnginnering: true);

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

    }
}
