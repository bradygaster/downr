using System.Collections.Generic;
using System.Linq;
using downr;
using downr.Models;
using downr.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace downr.Services
{
    public class PostFileSorter
    {
        private readonly ILogger<PostFileSorter> logger;
        private readonly DownrOptions downrOptions;
        public PostFileSorter(ILogger<PostFileSorter> logger, 
            IOptions<DownrOptions> downrOptions)
        {
            this.downrOptions = downrOptions.Value;
            this.logger = logger;
        }

        public List<Post> Sort(List<Post> posts)
        {
            if(downrOptions.SiteMode == SiteMode.Blog)
            {
                posts = posts.OrderByDescending(x => x.PublicationDate).ToList();
            }

            if(downrOptions.SiteMode == SiteMode.Workshop)
            {
                // strip out the posts that aren't in the workshop
                posts = posts.Where(x => x.Phase > 0 && x.Step > 0).ToList();

                // order the remaining posts
                posts = posts.OrderBy(x => x.Phase).ThenBy(x => x.Step).ToList();
            }
            
            return posts;
        }
    }
}