namespace VSProjectZip.Core.FileManagement;

public class FileImplementation : IFile
{
    public void Copy(string file, string destination, bool overwrite)
    {
        File.Copy(file, destination, overwrite);
    }

    public bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    public void Delete(string filePath)
    {
        File.Delete(filePath);
    }
}