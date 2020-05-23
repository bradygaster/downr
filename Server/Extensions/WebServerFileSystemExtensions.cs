using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace downr.Services
{
    public static class WebServerFileSystemExtensions
    {
        public static void WithWebServerFileSystemStorage(
            this DownrServicesCollectionExtensionsConfigurator configurator)
        {
            configurator.Services.AddSingleton<IYamlIndexer, WebServerFileSystemContentIndexer>();
        }

        public static void UseWebServerFileSystemStorage(
            this DownrContentProviderConfigurator configurator)
        {
            IWebHostEnvironment webHostEnvironment = (IWebHostEnvironment)
                configurator.Builder.ApplicationServices.GetService(typeof(IWebHostEnvironment));

            IYamlIndexer yamlIndexer = (IYamlIndexer)
                configurator.Builder.ApplicationServices.GetService(typeof(IYamlIndexer));

            if (string.IsNullOrWhiteSpace(webHostEnvironment.WebRootPath))
            {
                webHostEnvironment.WebRootPath = 
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            // index the content files once the site is ready
            var contentPath = Path.Combine(webHostEnvironment.WebRootPath, "posts");
            yamlIndexer.IndexContentFiles(contentPath);
        }
    }
}