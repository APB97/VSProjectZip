namespace VSProjectZip.Core.FileManagement;

public class DirectoryImplementation : IDirectory
{
    public bool Exists(string? path)
    {
        return Directory.Exists(path);
    }

    public void Delete(string path, bool recursive)
    {
        Directory.Delete(path, recursive);
    }
}
