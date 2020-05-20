using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SigSpec.Core;

namespace SigSpec.AspNetCore.Middlewares
{
    internal class SigSpecMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SigSpecSettings _settings;

        public SigSpecMiddleware(RequestDelegate next, SigSpecSettings settings)
        {
            _next = next;
            _settings = settings;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!RequestingSigSpecDocument(httpContext.Request))
            {
                await _next(httpContext);
                return;
            }

            await RespondWithSigSpecJsonAsync(httpContext.Response);
        }

        private bool RequestingSigSpecDocument(HttpRequest request)
        {
            if (request.Method != "GET")
            {
                return false;
            }

            if (request.Path.Value.Contains("sigspec/spec.json"))
            {
                return true;
            }

            return false;
        }

        private async Task RespondWithSigSpecJsonAsync(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json;charset=utf-8";

            var generator = new SigSpecGenerator(_settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            // TODO: Upgrade to .NET Core 3.1

            var hubs = _settings.Hubs
                .ToDictionary(k => k.Name.ToLower(), k => k);

            var document = await generator.GenerateForHubsAsync(hubs, _settings.Template);
            var json = document.ToJson();

            await response.WriteAsync(json, new UTF8Encoding(false));
        }
    }
}
