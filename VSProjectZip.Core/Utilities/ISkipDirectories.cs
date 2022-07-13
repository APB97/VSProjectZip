namespace VSProjectZip.Core.Utilities;

public interface ISkipDirectories
{
    IReadOnlySet<string> SkipTheseDirectories { get; }
    void AddDirectories(IEnumerable<string> additionalDirectoriesToSkip);
    void ClearDirectories();
}