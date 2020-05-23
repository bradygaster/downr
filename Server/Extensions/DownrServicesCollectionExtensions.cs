using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace downr.Services
{
    public static class DownrServicesCollectionExtensions
    {
        public static DownrServicesCollectionExtensionsConfigurator AddDownr(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<PostFileParser>();
            services.AddSingleton<PostService>();
            services.Configure<DownrOptions>(configuration.GetSection("downr"));
            
            return new DownrServicesCollectionExtensionsConfigurator(services, configuration);
        }
    }

    public class DownrServicesCollectionExtensionsConfigurator
    {
        internal DownrServicesCollectionExtensionsConfigurator(IServiceCollection services, 
            IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
        }

        public DownrServicesCollectionExtensionsConfigurator(IServiceCollection services) 
        {
            this.Services = services;
               
        }
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
    }
}