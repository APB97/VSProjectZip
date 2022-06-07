namespace VSProjectZip.Core.Utilities
{
    public class SkipNamesCopyUtility : CopyUtility
    {
        HashSet<string> skipTheseDirectories = new() { "bin", "obj", ".vs", ".git" };
        HashSet<string> skipTheseFiles = new() { ".gitattributes", ".gitignore" };

        public SkipNamesCopyUtility(IReadOnlySet<string>? additionalDirectoriesToSkip = null, IReadOnlySet<string>? additionalFilesToSkip = null)
        {
            if (additionalDirectoriesToSkip is not null)
            {
                foreach (var directory in additionalDirectoriesToSkip)
                {
                    skipTheseDirectories.Add(directory);
                }
            }

            if (additionalFilesToSkip is not null)
            {
                foreach (var file in additionalFilesToSkip)
                {
                    skipTheseFiles.Add(file);
                }
            }
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
