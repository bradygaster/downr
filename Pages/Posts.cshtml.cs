using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace downr.Pages
{
    public class PostsModel : PageModel
    {
        public PostsModel(ILogger<PostsModel> logger, 
            PostService postService)
        {
            Logger = logger;
            PostService = postService;
        }

        public string Slug { get; private set; }
        public ILogger<PostsModel> Logger { get; }
        public PostService PostService { get; }
        public Post Post { get; set; }
        public string NextPostSlug { get; set; }
        public string NextPostTitle { get; set; }
        public string PreviousPostSlug { get; set; }
        public string PreviousPostTitle { get; set; }

        public void OnGet(string slug)
        {
            Post = PostService.GetPostBySlug(slug);

            var prevNext = PostService.GetPreviousAndNextPosts(slug);

            if (prevNext.next != null)
            {
                NextPostSlug = prevNext.next.Slug;
                NextPostTitle = prevNext.next.Title;
            }

            if (prevNext.previous != null)
            {
                PreviousPostSlug = prevNext.previous.Slug;
                PreviousPostTitle = prevNext.previous.Title;
            }
        }
    }
}
