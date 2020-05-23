using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using downr;
using downr.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace downr.Services
{
    public class WebServerFileSystemContentIndexer : IYamlIndexer
    {
        private readonly ILogger _logger;
        public List<Post> Posts { get; set; }

        public WebServerFileSystemContentIndexer(ILogger<WebServerFileSystemContentIndexer> logger)
        {
            _logger = logger;
        }
        public Task IndexContentFiles(string contentPath)
        {
            _logger.LogInformation("Loading posts from disk...");
            
            Posts = Directory.GetDirectories(contentPath)
                                .Select(dir => Path.Combine(dir, "index.md"))
                                .Select(ParseMetadataPrivate)
                                .Where(m => m != null)
                                .OrderByDescending(x => x.PublicationDate)
                                .ToList();

            _logger.LogInformation("Loaded {0} posts", Posts.Count);

            return Task.CompletedTask;
        }

        public Task<Post> ReadPost(StreamReader postFileReader)
        {
            var post = PostFileParser.CreatePostFromReader(postFileReader);
            _logger.LogInformation($"Loaded post {post.Title}");
            return Task.FromResult<Post>(post);
        }

        private Post ParseMetadataPrivate(string indexFilePath)
        {
            using (var rdr = File.OpenText(indexFilePath))
            {
                return ReadPost(rdr).Result;
            }
        }
    }
}