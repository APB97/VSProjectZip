namespace VSProjectZip.Core.FileManagement;

public class FileImplementation : IFile
{
    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public void Delete(string filePath)
    {
        File.Delete(filePath);
    }
}
