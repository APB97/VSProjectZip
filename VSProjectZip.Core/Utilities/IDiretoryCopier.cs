namespace VSProjectZip.Core.Utilities
{
    public interface IDirectoryEnumerator
    {
        bool ShouldSkipDirectory(string directoryName);
        bool ShouldSkipFile(string fileName);
        IEnumerable<string> EnumerateEntries(string directory);
        IEnumerable<string> FilterOutSkippedItems(IEnumerable<string> files);
    }
}