namespace downr
{
    public static class Strings
    {
        public static class MetadataNames
        {
            public static string Slug { get { return "slug"; } }
            public static string Title { get { return "title"; } }
            public static string PublicationDate { get { return "pubDate"; } }
            public static string LastModified { get { return "lastModified"; } }
            public static string Author { get { return "author"; } }
            public static string Categories { get { return "categories"; } }
            public static string Description { get { return "description"; } }
            public static string Phase { get { return "phase"; } }
            public static string Step { get { return "step"; } }
        }
    }

    public static class StringExtensions
    {
        public static string StripLeading(this string value, string valueToStrip)
        {
            if (value.StartsWith(valueToStrip))
                return value.Substring(valueToStrip.Length);
            return value;
        }
        public static string EnsureTrailing(this string value, string valueToEndWith)
        {
            if (value.EndsWith(valueToEndWith))
                return value;
            return value + valueToEndWith;
        }
    }
}