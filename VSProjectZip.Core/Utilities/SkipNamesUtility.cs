using VSProjectZip.Core.FileManagement;

namespace VSProjectZip.Core.Utilities
{
    public class SkipNamesUtility(IFileSystem fileSystem) : DirectoryEnumerator(fileSystem), ISkipItems
    {
        public static readonly IReadOnlySet<string> DefaultDirectories = new HashSet<string>() { "bin", "obj", ".vs", ".git", "TestResults" };
        public static readonly IReadOnlySet<string> DefaultFiles = new HashSet<string>() { ".gitattributes", ".gitignore" };
        private readonly HashSet<string> _skipTheseDirectories = [.. DefaultDirectories];
        private readonly HashSet<string> _skipTheseFiles = [.. DefaultFiles];

        public IReadOnlySet<string> SkipTheseDirectories => _skipTheseDirectories;
        public IReadOnlySet<string> SkipTheseFiles => _skipTheseFiles;

        public void AddFiles(IEnumerable<string> additionalFilesToSkip)
        {
            foreach (var file in additionalFilesToSkip)
            {
                _skipTheseFiles.Add(file);
            }
        }

        public void AddDirectories(IEnumerable<string> additionalDirectoriesToSkip)
        {
            foreach (var directory in additionalDirectoriesToSkip)
            {
                _skipTheseDirectories.Add(directory);
            }
        }

        public void ClearFiles()
        {
            _skipTheseFiles.Clear();
        }

        public void ClearDirectories()
        {
            _skipTheseDirectories.Clear();
        }

        public override bool ShouldSkipDirectory(string directoryName)
        {
            return _skipTheseDirectories.Contains(Path.GetFileName(directoryName));
        }

        public override bool ShouldSkipFile(string fileName)
        {
            return _skipTheseFiles.Contains(Path.GetFileName(fileName));
        }
    }
}
