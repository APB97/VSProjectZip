namespace VSProjectZip.Core.Utilities
{
    public class SkipNamesCopyUtility : CopyUtility
    {
        public static readonly IReadOnlySet<string> DefaultDirectories = new HashSet<string>() { "bin", "obj", ".vs", ".git" };
        public static readonly IReadOnlySet<string> DefaultFiles = new HashSet<string>() { ".gitattributes", ".gitignore" };
        private readonly HashSet<string> skipTheseDirectories = new(DefaultDirectories);
        private readonly HashSet<string> skipTheseFiles = new(DefaultFiles);

        public IReadOnlySet<string> SkipTheseDirectories => skipTheseDirectories;
        public IReadOnlySet<string> SkipTheseFiles => skipTheseFiles;

        public void AddFiles(IEnumerable<string> additionalFilesToSkip)
        {
            foreach (var file in additionalFilesToSkip)
            {
                skipTheseFiles.Add(file);
            }
        }

        public void AddDirectories(IEnumerable<string> additionalDirectoriesToSkip)
        {
            foreach (var directory in additionalDirectoriesToSkip)
            {
                skipTheseDirectories.Add(directory);
            }
        }

        public void ClearFiles()
        {
            skipTheseFiles.Clear();
        }

        public void ClearDirectories()
        {
            skipTheseDirectories.Clear();
        }

        protected override bool ShouldSkipDirectory(string directoryName)
        {
            return skipTheseDirectories.Contains(directoryName);
        }

        protected override bool ShouldSkipFile(string fileName)
        {
            return skipTheseFiles.Contains(fileName);
        }
    }
}
