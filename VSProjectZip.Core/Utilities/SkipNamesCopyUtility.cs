using VSProjectZip.Core.FileManagement;

namespace VSProjectZip.Core.Utilities
{
    public class SkipNamesCopyUtility : CopyUtility, ISkipItems
    {
        public static readonly IReadOnlySet<string> DefaultDirectories = new HashSet<string>() { "bin", "obj", ".vs", ".git" };
        public static readonly IReadOnlySet<string> DefaultFiles = new HashSet<string>() { ".gitattributes", ".gitignore" };
        private readonly HashSet<string> _skipTheseDirectories = new(DefaultDirectories);
        private readonly HashSet<string> _skipTheseFiles = new(DefaultFiles);

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

        protected override bool ShouldSkipDirectory(string directoryName)
        {
            return _skipTheseDirectories.Contains(directoryName);
        }

        protected override bool ShouldSkipFile(string fileName)
        {
            return _skipTheseFiles.Contains(fileName);
        }

        public SkipNamesCopyUtility(IFileSystem fileSystem) : base(fileSystem)
        {
            
        }
    }
}
