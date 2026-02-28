using VSProjectZip.Core.FileManagement;

namespace VSProjectZip.Core.Utilities
{
    public class DirectoryEnumerator(IFileSystem fileSystem) : IDirectoryEnumerator
    {
        private readonly IDirectory _directory = fileSystem.Directory;

        public virtual bool ShouldSkipDirectory(string directoryName)
        {
            return false;
        }

        public virtual bool ShouldSkipFile(string fileName)
        {
            return false;
        }

        public IEnumerable<string> EnumerateEntries(string directory)
        {
            return Directory.EnumerateFileSystemEntries(directory, "*");
        }

        public IEnumerable<string> FilterOutSkippedItems(IEnumerable<string> files)
        {
            return files.Where(file =>
            {
                if (_directory.Exists(file) && ShouldSkipDirectory(file)) return false;
                if (ShouldSkipFile(file)) return false;
                return true;
            });
        }
    }
}
