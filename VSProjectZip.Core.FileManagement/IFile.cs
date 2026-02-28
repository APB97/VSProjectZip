namespace VSProjectZip.Core.FileManagement;

public interface IFile
{
    bool Exists(string filePath);
    void Delete(string filePath);
}
