using System;
using System.Collections.Generic;
using System.Linq;
using downr.Models;

namespace downr.Services
{
    public class PostService
    {
        private readonly IYamlIndexer _indexer;

        public PostService(IYamlIndexer indexer)
        {
            _indexer = indexer;
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
    }
}