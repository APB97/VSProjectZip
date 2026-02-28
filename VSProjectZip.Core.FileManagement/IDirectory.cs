namespace VSProjectZip.Core.FileManagement;

public interface IDirectory
{
    bool Exists(string? path);
    void Delete(string path, bool recursive);
}
