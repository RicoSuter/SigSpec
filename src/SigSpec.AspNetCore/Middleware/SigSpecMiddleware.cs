using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SigSpec.Core;

namespace SigSpec.AspNetCore.Middleware
{
    public class SigSpecMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SigSpecGeneratorSettings _settings;

        public SigSpecMiddleware(
            RequestDelegate next,
            SigSpecGeneratorSettings settings
           )
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

            await RespondWithSigSpecJson(httpContext.Response);
        }

        private bool RequestingSigSpecDocument(HttpRequest request)
        {
            if (request.Method != "GET") return false;
            if (request.Path.Value.Contains("sigspec/spec.json"))
            {
                return true;
            }

            return false;
        }

        private async Task RespondWithSigSpecJson(HttpResponse response)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json;charset=utf-8";

            var generator = new SigSpecGenerator(_settings);

            // TODO: Add PR to SignalR Core with new IHubDescriptionCollectionProvider service
            Dictionary<string, Type> hubsDict = _settings.Hubs.ToDictionary(k => k.Name.ToLower(), k => k);
            var document = await generator.GenerateForHubsAsync(hubsDict);

            var json = document.ToJson();

            await response.WriteAsync(json, new UTF8Encoding(false));
        }
    }
}
