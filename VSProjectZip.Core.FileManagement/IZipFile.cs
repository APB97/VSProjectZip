namespace VSProjectZip.Core.FileManagement;

public interface IZipFile
{
    void CreateFromDirectory(string path, string destinationFileName);
}