using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace SigSpec.AspNetCore.Middlewares
{
    internal class SigSpecMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SigSpecSettings _settings;
        private readonly IEnumerable<SigSpecDocumentRegistration> _documents;

        public SigSpecMiddleware(RequestDelegate next, SigSpecSettings settings, IEnumerable<SigSpecDocumentRegistration> documents)
        {
            _next = next;
            _settings = settings;
            _documents = documents;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method == "GET")
            {
                foreach (var document in _documents)
                {
                    if (httpContext.Request.Path.Value.Contains(_settings.Path.Replace("{documentName}", document.DocumentName)))
                    {
                        await RespondWithSigSpecJsonAsync(httpContext.Response, document);
                        return;
                    }
                }

                await _next(httpContext);
            }
            else
            {
                await _next(httpContext);
                return;
            }
        }

        private async Task RespondWithSigSpecJsonAsync(HttpResponse response, SigSpecDocumentRegistration registration)
        {
            response.StatusCode = 200;
            response.ContentType = "application/json;charset=utf-8";

            var document = await registration.GenerateDocumentAsync();
            await response.WriteAsync(document.ToJson(), new UTF8Encoding(false));
        }
    }
}
