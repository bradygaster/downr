using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using downr.Models;
using downr.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace downr.Services {

    public class AzureStorageConfiguration {
        public string ConnectionString { get; set; }
        public string Container { get; set; }
    }

    public class AzureStorageYamlIndexer : IYamlIndexer {
        private readonly ILogger<AzureStorageYamlIndexer> logger;
        private readonly AzureStorageConfiguration config; 
        public AzureStorageYamlIndexer (ILogger<AzureStorageYamlIndexer> logger,
            IOptions<AzureStorageConfiguration> config) {
            this.config = config.Value;
            this.logger = logger;
        }

        public List<Post> Posts { get; set; } = new List<Post> ();

        public async Task IndexContentFiles (string contentPath) 
        {
            BlobContainerClient container = 
                new BlobContainerClient (config.ConnectionString, config.Container);
            await container.CreateIfNotExistsAsync ();
        }

        public Task<Post> ReadPost (StreamReader postFileReader) 
        {
            throw new System.NotImplementedException ();
        }
    }
}