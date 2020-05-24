using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using downr.Models;

namespace downr.Services
{
    public interface IYamlIndexer
    {
        List<Post> Posts { get; set; }
        Task IndexContentFiles();
        Task<Post> ReadPost(StreamReader postFileReader);
    }
}