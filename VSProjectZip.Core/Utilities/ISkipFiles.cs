namespace VSProjectZip.Core.Utilities;

public interface ISkipFiles
{
    IReadOnlySet<string> SkipTheseFiles { get; }
    void AddFiles(IEnumerable<string> additionalFilesToSkip);
    void ClearFiles();
}