using System;
using Microsoft.Extensions.DependencyInjection;
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
    }
}
