using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using downr.Models;
using downr.Services;
using Microsoft.Extensions.Logging;

namespace downr.Services {

    public class AzureStorageConfiguration {
        public string ConnectionString { get; set; }
        public string Container { get; set; }
    }

    public class AzureStorageYamlIndexer : IYamlIndexer {
        private readonly ILogger<AzureStorageYamlIndexer> logger;
        private readonly AzureStorageConfiguration config; 
        public AzureStorageYamlIndexer (ILogger<AzureStorageYamlIndexer> logger,
            AzureStorageConfiguration config) {
            this.config = config;
            this.logger = logger;
        }

        public List<Post> Posts { get; set; } = new List<Post> ();

        public async Task IndexContentFiles (string contentPath) 
        {
            contentPath = config.Container;
            BlobContainerClient container = 
                new BlobContainerClient (config.ConnectionString, contentPath);
            await container.CreateIfNotExistsAsync ();
        }

        public Task<Post> ReadPost (StreamReader postFileReader) 
        {
            throw new System.NotImplementedException ();
        }
    }
}