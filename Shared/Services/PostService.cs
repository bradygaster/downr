using System;
using System.Collections.Generic;
using System.Linq;
using downr.Models;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    public class PostService
    {
        private readonly IYamlIndexer _indexer;
        private readonly DownrOptions _downrOptions;

        public PostService(IYamlIndexer indexer, IOptions<DownrOptions> downrOptions)
        {
            _downrOptions = downrOptions.Value;
            _indexer = indexer;
        }

        public Post[] GetPostsInCategory(string category)
        {
            var found = _indexer.Posts.Where(p => p.Categories.Contains(category)).ToArray();
            return found;
        }

        public Post[] GetPosts(int offset = 0, int count = -1, string category = null)
        {
            var posts = (IEnumerable<Post>)_indexer.Posts;

            if (category != null)
            {
                posts = posts.Where(p => p.Categories.Contains(category));
            }

            posts = posts.Skip(offset);

            if (count > 0)
            {
                posts = posts.Take(count);
            }
            
            return posts.ToArray();
        }

        public int GetNumberOfPosts(string category = null)
        {
            if (category != null)
            {
                return _indexer.Posts.Count(p => p.Categories.Contains(category));
            }
            return _indexer.Posts.Count;
        }

        public Post GetLatestPost()
        {
            return _indexer.Posts[0];
        }

        public Post GetPostBySlug(string slug)
        {
            return _indexer.Posts.FirstOrDefault(p => p.Slug == slug);
        }

        public (Post previous, Post next) GetPreviousAndNextPosts(string slug)
        {
            (Post previous, Post next) result = (null, null);

            int index = _indexer.Posts.FindIndex(x => x.Slug == slug);
            if (index != 0)
            {
                result.next = _indexer.Posts[index - 1];
            }
            if (index != GetNumberOfPosts() - 1)
            {
                result.previous = _indexer.Posts[index + 1];
            }
            return result;
        }

        public IEnumerable<string> GetCategories()
        {
            var categories = new List<string>();

            if(_downrOptions.SiteMode == SiteMode.Blog)
            {
                var categoryCounts = new Dictionary<string,int>();
                _indexer.Posts.ForEach(x => categories.AddRange(x.Categories));

                categories.ForEach(x => 
                {
                    if(categoryCounts.ContainsKey(x))
                    {
                        categoryCounts[x] += 1;
                    }
                    else
                    {
                        categoryCounts.Add(x, 1);
                    }
                });

                var sortedCategories = categoryCounts.OrderByDescending(x => x.Value);

                categories = sortedCategories.Select(x => x.Key.ToLower()).ToList();
            }

            // -------------------------------------------------------------
            // when in workshop mode order the category list by phase order
            // since, in workshop mode, the convention is to use Category
            // to identify the phase name
            // -------------------------------------------------------------
            if(_downrOptions.SiteMode == SiteMode.Workshop)
            {
                var categoriesByPhase = new Dictionary<string,int>();

                _indexer.Posts.ForEach(x => 
                {
                    if(!categoriesByPhase.ContainsKey(x.Categories[0]))
                    {
                        categoriesByPhase.Add(x.Categories[0], x.Phase);
                    }
                });
                
                var sortedCategories = categoriesByPhase.OrderBy(x => x.Value);

                categories = sortedCategories.Select(x => x.Key.ToLower()).ToList();
            }

            return categories;
        }
    }
}