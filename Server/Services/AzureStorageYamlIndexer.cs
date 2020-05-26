using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using downr.Models;
using downr.Services;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace downr.Services
{

    public class AzureStorageConfiguration
    {
        public string ConnectionString { get; set; }
        public string Container { get; set; }
    }

    public class AzureStorageYamlIndexer : IYamlIndexer
    {
        private readonly ILogger<AzureStorageYamlIndexer> logger;
        private readonly AzureStorageConfiguration config;
        public List<Post> Posts { get; set; } = new List<Post>();
        private readonly PostFileParser postFileParser;
        private readonly PostFileSorter postFileSorter;

        public AzureStorageYamlIndexer(ILogger<AzureStorageYamlIndexer> logger,
            IOptions<AzureStorageConfiguration> config,
            PostFileParser postFileParser,
            PostFileSorter postFileSorter)
        {
            this.postFileParser = postFileParser;
            this.config = config.Value;
            this.logger = logger;
            this.postFileSorter = postFileSorter;
        }

        public async Task IndexContentFiles()
        {
            this.Posts.Clear();
            
            BlobContainerClient container =
                new BlobContainerClient(config.ConnectionString, config.Container);

            await container.CreateIfNotExistsAsync();

            await foreach (BlobItem blobItem in container.GetBlobsAsync())
            {
                if (blobItem.Name.EndsWith("index.md"))
                {
                    logger.LogInformation($"Indexing {blobItem.Name}");
                    var blobClient = new BlobClient(config.ConnectionString, config.Container, blobItem.Name);
                    var reader = new StreamReader(blobClient.Download().Value.Content);
                    var post = ReadPost(reader).Result;
                    Posts.Add(post);
                }
            }

            this.Posts = postFileSorter.Sort(this.Posts);
        }

        public Task<Post> ReadPost(StreamReader postFileReader)
        {
            var post = postFileParser.CreatePostFromReader(postFileReader);
            logger.LogInformation($"Indexed post {post.Title}");
            return Task.FromResult<Post>(post);
        }
    }
}