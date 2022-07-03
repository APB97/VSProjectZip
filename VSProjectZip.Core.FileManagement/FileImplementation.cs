namespace VSProjectZip.Core.FileManagement;

public class FileImplementation : IFile
{
    public void Copy(string file, string destination, bool overwrite)
    {
        File.Copy(file, destination, overwrite);
    }
}