namespace VSProjectZip.Core.FileManagement;

public interface IPath
{
    string? GetFileName(string file);
    string GetRelativePath(string source, string file);
    string Combine(string destination, string relativePath);
    string Combine(params string[] paths);
    string? GetDirectoryName(string destinationFileName);
}