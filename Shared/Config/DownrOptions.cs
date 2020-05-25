namespace downr
{
    public class DownrOptions
    {
        /// <summary>
        /// The title for the blog (used in feed)
        /// </summary>
        /// <value></value>
        public string Title { get; set; }

        /// <summary>
        /// The external root of the blog (e.g. http://example.com/blog)
        /// </summary>
        /// <value></value>
        public string RootUrl { get; set; }

        /// <summary>
        /// PageSize, i.e. the number of items to show per page in a paged list
        /// </summary>
        /// <value></value>
        public int PageSize { get; set; }

        /// <summary>
        /// the name of the main author of the site
        /// </summary>
        /// <value></value>
        public string Author { get; set; }

        /// <summary>
        /// the text displayed on the index page if the index page isn't edited from the default content
        /// </summary>
        /// <value></value>
        public string IndexPageText { get; set; }

        /// <summary>
        /// During post-file parsing, the image path is repaired, as during editing all image 
        /// paths should simply be marked as "media/{path-to}.png" (or whatever other file format). 
        /// This configuration setting allows you to create your own value. 
        /// 
        /// This is useful in scenarios where you want to serve images not from your local file 
        /// system, but from a CDN. 
        /// 
        /// The default value without being configured is "/posts/{0}/media/".
        /// </summary>
        /// <value></value>
        public string ImagePathFormat { get; set; } = "/posts/{0}/media/";

        /// <summary>
        /// This value is optional. If it is provided in your configuration, the Google tracking 
        /// JavaScript code will be injected into your pages. 
        /// </summary>
        /// <value></value>
        public string GoogleTrackingCode { get; set; }

        /// <summary>
        /// Use this property in the downr configuration to control how often 
        /// the content is refreshed from disk or from Azure Storage. This property 
        /// reflects the number of minutes in between each content refresh. 
        /// 
        /// When set to 0 or omitted from appsettings.json, auto-refresh is disabled.
        /// </summary>
        /// <value></value>
        public int AutoRefreshInterval { get; set; } = 0;

        /// <summary>
        /// The site mode, which can either be set to "Blog" (the default) or "Workshop." 
        /// When in Workshop mode, posts will need to have a "Phase" and a "Step" property identified,
        /// otherwise they will be omitted during indexing.
        /// 
        /// When in Blog mode, the posts will be shown chronologically, newest-to-oldest.
        /// 
        /// When in Workshop mode, the posts will be shown ordered by their Phase and Step 
        /// properties. They'll be sorted first by Phase, then by Step, as each workshop Phase
        /// will have multiple steps.
        /// </summary>
        /// <value></value>
        public SiteMode SiteMode { get; set; } = SiteMode.Blog;

        /// <summary>
        /// If this is omitted from appsettings.json the image will be hidden.
        /// </summary>
        /// <value></value>
        public string HeaderImage { get; set; }
    }

    public enum SiteMode : int
    {
        Blog = 0,
        Workshop = 1
    }
}