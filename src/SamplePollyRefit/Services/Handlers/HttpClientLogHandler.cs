using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SamplePollyRefit.Services.Handlers
{
    public class HttpClientLogHandler : DelegatingHandler
    {
        private readonly ILogger<HttpClientLogHandler> _logger;

        public HttpClientLogHandler(ILogger<HttpClientLogHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ConfigureHeaders(request);

            LogRequest(request);
            var response = await base.SendAsync(request, cancellationToken);
            LogResponse(request, response);

            return response;
        }

        private void ConfigureHeaders(HttpRequestMessage request)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("User-Agent", "request");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        }

        private async void LogRequest(HttpRequestMessage request)
            => _logger.LogInformation("Efetuando request para {url} {headers} {body}", request.RequestUri, JsonConvert.SerializeObject(request.Headers),
                request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty);

        private async void LogResponse(HttpRequestMessage request, HttpResponseMessage response)
            => _logger.LogInformation("Resposta da chama para {url} {statusCode} {body}", request.RequestUri, response.StatusCode,
                request.Content != null ? await request.Content.ReadAsStringAsync() : string.Empty);

    }

}
