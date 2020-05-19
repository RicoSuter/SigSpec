using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SigSpec.AspNetCore.Middleware;
using SigSpec.Core;

namespace Microsoft.AspNetCore.Builder
{
    public static class SigSpecApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSigSpec(this IApplicationBuilder app, Action<SigSpecGeneratorSettings> setupAction = null)
        {
            var options = new SigSpecGeneratorSettings();
            if (setupAction != null)
            {
                setupAction(options);
            }
            else
            {
                options = app.ApplicationServices.GetRequiredService<IOptions<SigSpecGeneratorSettings>>().Value;
            }

            app.UseMiddleware<SigSpecMiddleware>(options);

            return app;
        }
    }
}
