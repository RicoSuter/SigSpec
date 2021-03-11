using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SigSpec.AspNetCore.UI;

namespace HelloSignalR
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            // TODO: Automatically look hubs up
            services.AddSigSpecDocument(o => o.Hubs["/chat"] = typeof(ChatHub));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
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
            services.ConfigureOptions(typeof(UIConfigureOptions));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();
            //app.UseCors();

            app.UseSigSpec();
            //app.UseSigSpecUi();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute("sigSpecui", "sigSpecui","{area}/{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
