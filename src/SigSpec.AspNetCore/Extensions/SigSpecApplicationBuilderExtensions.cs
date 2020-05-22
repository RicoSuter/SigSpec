using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SigSpec.AspNetCore;
using SigSpec.AspNetCore.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class SigSpecApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSigSpec(this IApplicationBuilder app, Action<SigSpecSettings> configure = null)
        {
            var settings = app.ApplicationServices.GetService<IOptions<SigSpecSettings>>()?.Value;
            if (settings == null)
            {
                settings = new SigSpecSettings();
                configure?.Invoke(settings);
            }

            var documents = app.ApplicationServices.GetServices<SigSpecDocumentRegistration>();
            app.UseMiddleware<SigSpecMiddleware>(settings, documents);
            return app;
        }

        public static IApplicationBuilder UseSigSpecUi(this IApplicationBuilder app, Action<SigSpecUiSettings> configure = null)
        {
            var settings = new SigSpecUiSettings();
            configure?.Invoke(settings);

            // TODO: Redirect /sigspec to /sigspec/index.html
            // TODO: Inject URLs and document names from registered documents into index.html => and then JS

            app.UseFileServer(new FileServerOptions
            {
                RequestPath = new PathString(settings.Route),
                FileProvider = new EmbeddedFileProvider(typeof(SigSpecApplicationBuilderExtensions).GetTypeInfo().Assembly, "SigSpec.AspNetCore.SigSpecUi")
            });

            return app;
        }
    }
}
