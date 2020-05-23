using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    public static class AzureStorageExtensions
    {
        public static void WithAzureStorage(
            this DownrServicesCollectionExtensionsConfigurator configurator)
        {
            configurator.Services.AddSingleton<IYamlIndexer, AzureStorageYamlIndexer>();
            configurator.Services.AddSingleton<PostService>();
            configurator.Services.Configure<DownrOptions>(configurator.Configuration.GetSection("downr"));
            configurator.Services.Configure<AzureStorageConfiguration>(configurator.Configuration.GetSection("downr.AzureStorage"));
        }

        public static void UseAzureStorage(
            this DownrContentProviderConfigurator configurator)
        {
            IYamlIndexer yamlIndexer = (IYamlIndexer)
                configurator.Builder.ApplicationServices.GetService(typeof(IYamlIndexer));

            IOptions<AzureStorageConfiguration> config = (IOptions<AzureStorageConfiguration>)
                configurator.Builder.ApplicationServices.GetService(typeof(IOptions<AzureStorageConfiguration>));
            
            yamlIndexer.IndexContentFiles(config.Value.Container);
        }
    }
}