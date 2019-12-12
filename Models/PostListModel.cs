namespace downr.Models
{
    public class PostListModel
    {
        public Post[] Posts { get; set; }
        public string NextPageLink { get; set; }
        public string PreviousPageLink { get; set; }
    }
}