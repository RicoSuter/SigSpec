using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

        public static void AddHubsFromSigSpec(this IEndpointRouteBuilder endpoints, IApplicationBuilder app)
        {
            var method = typeof(HubEndpointRouteBuilderExtensions)
                .GetMethod("MapHub", new[] { typeof(IEndpointRouteBuilder), typeof(string) });

            var registrations = app.ApplicationServices.GetServices<SigSpecDocumentRegistration>();
            foreach (var registration in registrations
                .SelectMany(r => r.Hubs)
                .GroupBy(p => p.Key)
                .Select(p => p.First())
                .ToDictionary(p => p.Key, p => p.Value))
            {
                var specificMethod = method.MakeGenericMethod(new[] { registration.Value });
                specificMethod.Invoke(null, new object[] { endpoints, registration.Key });
            }
        }

        /// <summary>
        /// Experimental: Handles the --sigspec command line argument and allows extension points to run SigSpec actions.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="processDocument"></param>
        /// <returns></returns>
        public static IApplicationBuilder HandleSigSpecCommandLine(this IApplicationBuilder app)
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Contains("--sigspec"))
            {
                var logger = app.ApplicationServices.GetRequiredService<ILogger<SigSpecDocumentRegistration>>();
                var registrations = app.ApplicationServices.GetServices<SigSpecDocumentRegistration>();
                foreach (var registration in registrations.Where(r => !string.IsNullOrEmpty(r.OutputPath) || r.CommandLineAction != null))
                {
                    var document = registration.GenerateDocumentAsync().GetAwaiter().GetResult();
                    registration.CommandLineAction?.Invoke(document);

                    if (!string.IsNullOrEmpty(registration.OutputPath))
                    {
                        File.WriteAllText(registration.OutputPath, document.ToJson());
                        logger.LogInformation("SigSpec document {DocumentName} successfully generated and written to file system {OutputPath}.", registration.DocumentName, registration.OutputPath);
                    }
                }

                var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
                lifetime.StopApplication();
            }

            return app;
        }
    }
}
