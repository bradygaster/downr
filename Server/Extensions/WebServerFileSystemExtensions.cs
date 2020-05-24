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
            IYamlIndexer yamlIndexer = (IYamlIndexer)
                configurator.Builder.ApplicationServices.GetService(typeof(IYamlIndexer));

            yamlIndexer.IndexContentFiles();
        }
    }
}