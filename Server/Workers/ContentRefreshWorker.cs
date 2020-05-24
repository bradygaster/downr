using System;
using System.Threading;
using System.Threading.Tasks;
using downr.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace downr.Workers {
    public class ContentRefreshWorker : BackgroundService {
        private readonly ILogger<ContentRefreshWorker> logger;
        private readonly IYamlIndexer yamlIndexer;
        private readonly DownrOptions config; 

        public ContentRefreshWorker (ILogger<ContentRefreshWorker> logger,
            IYamlIndexer yamlIndexer, 
            IOptions<DownrOptions> config) 
        {
            this.config = config.Value;
            this.yamlIndexer = yamlIndexer;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync (CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested) 
            {
                logger.LogInformation ("Time to refresh the content...");
                await yamlIndexer.IndexContentFiles ();
                await Task.Delay (TimeSpan.FromMinutes(config.AutoRefreshInterval));
            }
        }
    }
}