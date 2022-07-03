namespace VSProjectZip.Core.FileManagement;

public interface IDirectory
{
    bool Exists(string? path);
    IDirectoryInfo CreateDirectory(string path);
    IEnumerable<string> GetDirectories(string directory);
    IEnumerable<string> GetFiles(string directory);
    void Delete(string path, bool recursive);
}