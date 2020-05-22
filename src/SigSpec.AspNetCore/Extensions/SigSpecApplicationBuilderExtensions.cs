using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SigSpec.AspNetCore.Middlewares;

namespace Microsoft.AspNetCore.Builder
{
    public static class SigSpecApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSigSpec(this IApplicationBuilder app, Action<SigSpecSettings> setupAction = null)
        {
            var options = new SigSpecSettings();

            if (setupAction != null)
            {
                setupAction(options);
            }
            else
            {
                options = app.ApplicationServices.GetRequiredService<IOptions<SigSpecSettings>>().Value;
            }

            app.UseMiddleware<SigSpecMiddleware>(options);
            return app;
        }

        public static IApplicationBuilder UseSigSpecUi(this IApplicationBuilder app, Action<SigSpecSettings> configure = null)
        {
            var settings = configure == null ? app.ApplicationServices.GetService<IOptions<SigSpecSettings>>()?.Value : null ?? new SigSpecSettings();
            app.UseMiddleware<SwaggerUiIndexMiddleware>("sigspec/index.html", settings, "SigSpec.AspNetCore.SigSpecUI.index.html");

            app.UseFileServer(new FileServerOptions
            {
                RequestPath = new PathString("/sigspec"),
                FileProvider = new EmbeddedFileProvider(typeof(SigSpecApplicationBuilderExtensions).GetTypeInfo().Assembly, "SigSpec.AspNetCore.SigSpecUI")
            });


            return app;
        }
    }
}
