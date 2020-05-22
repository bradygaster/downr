namespace downr
{
    public class DownrOptions
    {
        // The title for the blog (used in feed)
        public string Title { get; set; }

        // The external root of the blog (e.g. http://example.com/blog)
        public string RootUrl { get; set; }

        // PageSize, i.e. the number of items to show per page in a paged list
        public int PageSize { get; set; }

        // the name of the main author of the site
        public string Author { get; set; }

        // the text displayed on the index page if the index page isn't edited from the default content
        public string IndexPageText { get; set; }
    }

    public enum HomePageStyle
    {
        LatestPost,
        SummaryList
    }
}