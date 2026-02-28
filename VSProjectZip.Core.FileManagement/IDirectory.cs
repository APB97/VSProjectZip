namespace VSProjectZip.Core.FileManagement;

public interface IDirectory
{
    bool Exists(string? path);
    IDirectoryInfo CreateDirectory(string path);
    void Delete(string path, bool recursive);
}