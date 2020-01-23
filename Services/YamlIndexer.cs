using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using downr.Models;
using YamlDotNet.Serialization;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Markdig;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    public class YamlIndexer
    {
        private readonly ILogger _logger;
        private readonly IOptions<DownrOptions> _options;

        public List<Post> Metadata { get; set; }


        public YamlIndexer(ILogger<YamlIndexer> logger,
            IOptions<DownrOptions> options)
        {
            _logger = logger;
            _options = options;
        }
        public void IndexContentFiles(string contentPath)
        {
            Metadata = LoadMetadata(contentPath);
        }

        private List<Post> LoadMetadata(string contentPath)
        {
            _logger.LogInformation("Loading post content...");
            List<Post> list = Directory.GetDirectories(contentPath)
                                .Select(dir => Path.Combine(dir, "index.md"))
                                .Select(ParseMetadata)
                                .Where(m => m != null)
                                .OrderByDescending(x => x.PublicationDate)
                                .ToList();

            if(_options.Value.OldestOnTop) list.Reverse();

            _logger.LogInformation("Loaded {0} posts", list.Count);
            return list;
        }

        private Post ParseMetadata(string indexFile)
        {
            var deserializer = new Deserializer();
            
            using (var rdr = File.OpenText(indexFile))
            {
                // make sure the file has the header at the first line
                var line = rdr.ReadLine();
                if (line == "---")
                {
                    line = rdr.ReadLine();

                    var stringBuilder = new StringBuilder();

                    // keep going until we reach the end of the header
                    while (line != "---")
                    {
                        stringBuilder.Append(line);
                        stringBuilder.Append("\n");
                        line = rdr.ReadLine();
                    
                    }

                    var htmlContent = rdr.ReadToEnd().TrimStart('\r', '\n', '\t', ' ');
                    htmlContent = Markdig.Markdown.ToHtml(htmlContent);

                    var yaml = stringBuilder.ToString();
                    var result = deserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                    // convert the dictionary into a model
                    var slug = result[Strings.MetadataNames.Slug];
                    htmlContent = FixUpImageUrls(htmlContent, slug);

                    try
                    {
                        var metadata = new Post
                        {
                            Slug = slug,
                            Title = result[Strings.MetadataNames.Title],
                            Author = result[Strings.MetadataNames.Author],
                            PublicationDate = DateTime.Parse(result[Strings.MetadataNames.PublicationDate]),
                            LastModified = DateTime.Parse(result[Strings.MetadataNames.LastModified]),
                            Description = result[Strings.MetadataNames.Description],
                            Categories = result[Strings.MetadataNames.Categories
                                                ]?.Split(',')
                                                .Select(c => c.Trim())
                                                .ToArray()
                                                ?? new string[] { },
                            Content = htmlContent
                        };

                        return metadata;
                    }
                    catch
                    {
                        Console.WriteLine($"No description in {slug}");
                    }
                }
            }
            return null;
        }

        private static string FixUpImageUrls(string html, string slug)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    var src = node.Attributes["src"].Value;
                    src = src.Replace("media/", string.Format("/posts/{0}/media/", slug));
                    node.SetAttributeValue("src", src);
                }
            }

            html = htmlDoc.DocumentNode.OuterHtml;

            return html;
        }
    }
}