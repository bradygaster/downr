using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace downr.Services
{
    public static class DownrApplicationBuilderExtensions
    {
        public static DownrContentProviderConfigurator UseDownr(this IApplicationBuilder builder)
        {
            return new DownrContentProviderConfigurator(builder);
        }
    }



    public class DownrContentProviderConfigurator
    {
        public IApplicationBuilder Builder { get; private set; }

        internal DownrContentProviderConfigurator(IApplicationBuilder builder)
        {
            Builder = builder;
        }
    }
}