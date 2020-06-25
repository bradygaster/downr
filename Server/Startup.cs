using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using downr.Services;
using Microsoft.Net.Http.Headers;
using System.IO;
using Microsoft.Extensions.Options;

namespace downr.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] 
                {   
                    "image/svg+xml", 
                    "application/font-woff2" 
                });
            });

            // Add framework services.
            services.AddMvc();

            // add downr 
            services.AddDownr(Configuration)
                    //.WithAzureStorage();
                    .WithWebServerFileSystemStorage();
            
            // add asp.net services
            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        public void Configure(IApplicationBuilder app, 
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseResponseCompression();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = _ => _.Context.Response.Headers[HeaderNames.CacheControl] = "public,max-age=604800"
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.MapControllerRoute(
                    name: "blog-feed-rss",
                    pattern: "/feed/rss",
                    defaults: new { controller = "Feed", Action = "Rss" }
                );
            });

            app.UseDownr()
                //.UseAzureStorage();
                .UseWebServerFileSystemStorage();
        }
    }
}
