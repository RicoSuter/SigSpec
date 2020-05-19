using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddCors(c =>
            {
                c.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseStaticFiles();

            app.UseCors();

            app.UseSigSpec(options => { options.Hubs = new List<Type>() {typeof(ChatHub)}; });

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });
        }
    }
}
