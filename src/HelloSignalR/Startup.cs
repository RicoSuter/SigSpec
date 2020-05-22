using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace HelloSignalR
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            // TODO: Automatically look hubs up
            services.AddSigSpecDocument(o => o.Hubs["/chat"] = typeof(ChatHub));

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

            app.UseSigSpec();
            app.UseSigSpecUi();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });
        }
    }
}
