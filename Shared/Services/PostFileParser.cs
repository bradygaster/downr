using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using downr.Models;
using HtmlAgilityPack;
using Markdig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;

namespace downr.Services
{
    public class PostFileParser
    {
        private readonly ILogger<PostFileParser> logger;
        private readonly DownrOptions downrOptions;

        public PostFileParser(ILogger<PostFileParser> logger, 
            IOptions<DownrOptions> downrOptions
        )
        {
            this.logger = logger;
            this.downrOptions = downrOptions.Value;
        }

        public Post CreatePostFromReader(StreamReader postReader)
        {
            var deserializer = new Deserializer();
            
            // make sure the file has the header at the first line
            var line = postReader.ReadLine();
            if (line == "---")
            {
                line = postReader.ReadLine();

                var stringBuilder = new StringBuilder();

                // keep going until we reach the end of the header
                while (line != "---")
                {
                    stringBuilder.Append(line);
                    stringBuilder.Append("\n");
                    line = postReader.ReadLine();
                }

                var htmlContent = postReader.ReadToEnd().TrimStart('\r', '\n', '\t', ' ');

                htmlContent = Markdig.Markdown.ToHtml(htmlContent);

                var yaml = stringBuilder.ToString();
                var result = deserializer.Deserialize<Dictionary<string, string>>(new StringReader(yaml));

                // convert the dictionary into a model
                var slug = result[Strings.MetadataNames.Slug];
                htmlContent = FixUpImageUrls(htmlContent, slug);

                try
                {
                    var post = new Post
                    {
                        Slug = slug,
                        Title = result[Strings.MetadataNames.Title],
                        Author = result[Strings.MetadataNames.Author],
                        PublicationDate = DateTime.Parse(result[Strings.MetadataNames.PublicationDate]),
                        LastModified = DateTime.Parse(result[Strings.MetadataNames.LastModified]),
                        Description = result[Strings.MetadataNames.Description],
                        Categories = result[Strings.MetadataNames.Categories
                                            ]?.Split(',')
                                            .Select(c => c.Trim().ToLower())
                                            .ToArray()
                                            ?? new string[] { },
                        Content = htmlContent
                    };

                    if(downrOptions.SiteMode == SiteMode.Workshop)
                    {
                        if(result.ContainsKey(Strings.MetadataNames.Phase) && 
                            result.ContainsKey(Strings.MetadataNames.Step))
                        {
                            post.Phase = Convert.ToInt32(result[Strings.MetadataNames.Phase]);
                            post.Step = Convert.ToInt32(result[Strings.MetadataNames.Step]);
                        }
                        else
                        {
                            return post; // if we're in workshop mode, we need phases & steps
                        }
                    }

                    return post;
                }
                catch
                {
                    logger.LogError($"No description in {slug}");
                }
            }

            return null;
        }

        internal string FixUpImageUrls(string html, 
            string slug)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//img[@src]");
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    var src = node.Attributes["src"].Value;
                    src = src.Replace("media/", string.Format(downrOptions.ImagePathFormat, slug));
                    node.SetAttributeValue("src", src);
                }
            }

            html = htmlDoc.DocumentNode.OuterHtml;

            return html;
        }
    }
}