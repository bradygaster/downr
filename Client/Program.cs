using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Blazored.LocalStorage;
using downr.Services;
using System.Reflection;

namespace downr.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddSingleton<PostService>();

            builder.Services.AddTransient(sp => new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

#if DEBUG
            string fileName = "downr.Client.appsettings.Development.json";
#else
            string fileName = "downr.Client.appsettings.json";
#endif
            
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName);
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();

            builder.Services.AddTransient(_ =>  
            { 
                return config.GetSection("downr").Get<DownrOptions>(); 
            });

            await builder.Build().RunAsync();
        }
    }
}
