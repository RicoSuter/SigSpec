using Microsoft.AspNetCore.Http;
using SigSpec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SigSpec.Json.Middleware
{
    public class SigSpecMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEnumerable<Type> hubs;
        private readonly SigSpecGeneratorSettings settings;

        public SigSpecMiddleware(
            RequestDelegate next,
            List<Type> hubs,
            SigSpecGeneratorSettings settings
           )
        {
            _next = next;
            this.hubs = hubs;
            this.settings = settings;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!RequestingSigSpecDocument(httpContext.Request))
            {
                await _next(httpContext);
                return;
            }

            await RespondWithSigSpecJson(httpContext.Response);
        }

        private bool RequestingSigSpecDocument(HttpRequest request)
        {
            if (request.Method != "GET") return false;
            if (request.PathBase.Value.Contains("sigspec") && request.Path.Value.Contains("spec.json"))
            {
                return true;
            }

            return false;
        }

        private void RespondWithNotFound(HttpResponse response)
        {
            response.StatusCode = 404;
        }

        private async Task RespondWithSigSpecJson(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json;charset=utf-8";

            var generator = new SigSpecGenerator(settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            Dictionary<string, Type> hubsDict = hubs.ToDictionary(k => k.Name.ToLower(), k => k);
            var document = await generator.GenerateForHubsAsync(hubsDict);

            var json = document.ToJson();

            await response.WriteAsync(json, new UTF8Encoding(false));
        }
    }
}
