using System.IO;

namespace downr.Services
{
    public class PostFolderWatcher
    {
        private readonly FileSystemWatcher _watcher;

        public PostFolderWatcher()
        {
            _watcher = new FileSystemWatcher("posts");
        }
    }
}