namespace VSProjectZip.Core.Parsing;

public interface ISkippedItemsUpdater
{
    void UpdateSkippedFiles(IReadOnlyDictionary<string, string?> argumentValues);
    void UpdateSkippedDirectories(IReadOnlyDictionary<string, string?> argumentValues);
}