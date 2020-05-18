using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SigSpec.Core;
using System;
using System.Collections.Generic;

namespace SigSpec.Middleware
{
    public static class SigSpecBuilderExtensions
    {
        public static IApplicationBuilder UseSigSpec(this IApplicationBuilder app, IEnumerable<Type> hubs, Action<SigSpecGeneratorSettings> setupAction = null)
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

            app.UseMiddleware<SigSpecMiddleware>(hubs, options);

            return app;
        }
    }
}
