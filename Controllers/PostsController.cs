using System;
using System.Collections.Generic;
using System.Linq;
using downr;
using downr.Models;
using downr.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace downr.Controllers
{
    public class PostsController : Controller
    {
        private readonly DownrOptions _options;
        private readonly PostService _postService;

        public PostsController(
            PostService postService,
            IOptions<DownrOptions> options
            )
        {
            _postService = postService;
            _options = options.Value;
        }

        public IActionResult Category(string name, int page = 1)
        {
            var model = new CategoryPostListModel
            {
                CategoryName = name,
                PostList = GetPostList(page, category: name),
            };
            return View("CategoryPostList", model);
        }

        private PostListModel GetPostList(int page, string category = null)
        {
            var pageSize = _options.PageSize;
            var pageIndex = page - 1;
            var posts = _postService.GetPosts(pageIndex * pageSize, pageSize, category);
            var postCount = _postService.GetNumberOfPosts(category);

            var pagingFunction = (category == null) ? (Func<int, string, string>)GetPagedIndexLink : GetPagedCategoryLink;
            var model = new PostListModel
            {
                Posts = posts,
                NextPageLink = (page > 1) ? pagingFunction(page - 1, category) : null,
                PreviousPageLink = (postCount > (pageIndex + 1) * pageSize) ? pagingFunction(page + 1, category) : null
            };
            return model;
        }

        private string GetPagedIndexLink(int page, string category)
        {
            if (page > 1)
                return Url.Action("Index", new { page });
            if (page == 1)
                return Url.Action("Index"); // page defaults to 1 so keep URL clean :-)

            throw new ArgumentException("page must be greater than or equal to one");
        }
        private string GetPagedCategoryLink(int page, string category)
        {
            if (page > 1)
                return Url.Action("Category", new { page, nameof = category });
            if (page == 1)
                return Url.Action("Category", new { name = category }); // page defaults to 1 so keep URL clean :-)

            throw new ArgumentException("page must be greater than or equal to one");
        }
    }
}
