using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema.CodeGeneration.TypeScript;
using SigSpec.CodeGeneration.TypeScript;
using System.IO;

namespace HelloSignalR
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();

            // TODO: Automatically look hubs up
            services.AddSigSpecDocument(options =>
            {
                options.Hubs["/chat"] = typeof(ChatHub);

                options.OutputPath = "sigspec.json";
                options.CommandLineAction = document => // run cli with "donet run -- --sigspec""
                {
                    var generator = new SigSpecToTypeScriptGenerator(
                        new SigSpecToTypeScriptGeneratorSettings
                        {
                            TypeScriptGeneratorSettings =
                            {
                                TypeStyle = TypeScriptTypeStyle.Interface
                            }
                        });

                    var code = generator.GenerateFile(document);
                    File.WriteAllText("signalr-api.ts", code);
                };
            });

            services.AddCors(c =>
            {
                c.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();
            app.UseCors();

            app.UseSigSpec();
            app.UseSigSpecUi();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.AddHubsFromSigSpec(app);
            });

            app.HandleSigSpecCommandLine();
        }
    }
}
