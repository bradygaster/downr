namespace downr.Models
{
    public class PostPageModel
    {
        public Post Post { get; set; }
        public Post NextPost { get; set; }
        public Post PreviousPost { get; set; }
    }
}