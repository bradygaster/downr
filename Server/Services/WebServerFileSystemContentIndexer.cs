using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using downr;
using downr.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace downr.Services
{
    public class WebServerFileSystemContentIndexer : IYamlIndexer
    {
        private readonly ILogger logger;
        public List<Post> Posts { get; set; }
        private readonly PostFileParser postFileParser;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly PostFileSorter postFileSorter;

        public WebServerFileSystemContentIndexer(ILogger<WebServerFileSystemContentIndexer> logger,
            PostFileParser postFileParser,
            IWebHostEnvironment webHostEnvironment,
            PostFileSorter postFileSorter)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.postFileParser = postFileParser;
            this.logger = logger;
            this.postFileSorter = postFileSorter;
        }
        public Task IndexContentFiles()
        {
            logger.LogInformation("Loading posts from disk...");
            this.Posts.Clear();

            if (string.IsNullOrWhiteSpace(webHostEnvironment.WebRootPath))
            {
                webHostEnvironment.WebRootPath = 
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            
            var contentPath = Path.Combine(webHostEnvironment.WebRootPath, "posts");

            Posts = Directory.GetDirectories(contentPath)
                                .Select(dir => Path.Combine(dir, "index.md"))
                                .Select(ParseMetadataPrivate)
                                .Where(m => m != null)
                                .OrderByDescending(x => x.PublicationDate)
                                .ToList();

            logger.LogInformation("Loaded {0} posts", Posts.Count);

            this.Posts = postFileSorter.Sort(this.Posts);

            return Task.CompletedTask;
        }

        public Task<Post> ReadPost(StreamReader postFileReader)
        {
            var post = postFileParser.CreatePostFromReader(postFileReader);
            logger.LogInformation($"Loaded post {post.Title}");
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