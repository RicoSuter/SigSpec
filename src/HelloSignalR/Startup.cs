using HelloSignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace HelloSignalR
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            app.UseSignalRWithSpecification(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });
        }
    }

    internal class SigSpecHubConfiguration
    {
        private readonly Dictionary<string, Type> _hubs = new Dictionary<string, Type>();

        IReadOnlyDictionary<string, Type> Hubs => _hubs;

        public void AddHub(string path, Type type)
        {
            _hubs[path] = type;
        }
    }
}

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SigSpecServiceCollectionExtensions
    {
        public static IServiceCollection AddSigSpec(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<SigSpecHubConfiguration>();
            return serviceCollection;
        }
    }
}

namespace Microsoft.AspNetCore.Builder
{
    public static class SigSpecAppBuilderExtensions
    {
        public static IApplicationBuilder UseSignalRWithSpecification(this IApplicationBuilder app, Action<HubRouteWithSpecificationBuilder> configure)
        {
            var specificationBuilder = new HubRouteWithSpecificationBuilder(app.ApplicationServices.GetRequiredService<SigSpecHubConfiguration>());
            return app.UseSignalR(builder =>
            {
                specificationBuilder.Builder = builder;
                configure(specificationBuilder);
            });
        }
    }

    public class HubRouteWithSpecificationBuilder
    {
        private readonly SigSpecHubConfiguration _configuration;

        internal HubRouteWithSpecificationBuilder(SigSpecHubConfiguration configuration)
        {
            _configuration = configuration;
        }

        internal HubRouteBuilder Builder { get; set; }

        public string SpecificationPath { get; set; }

        public void MapHub<THub>(PathString path) where THub : Hub
        {
            Builder.MapHub<THub>(path);
            _configuration.AddHub(path, typeof(THub));
        }

        public void MapHub<THub>(PathString path, Action<HttpConnectionDispatcherOptions> configureOptions) where THub : Hub
        {
            Builder.MapHub<THub>(path, configureOptions);
            _configuration.AddHub(path, typeof(THub));
        }
    }
}
