namespace VSProjectZip.Core.FileManagement;

public class DirectoryImplementation : IDirectory
{
    public bool Exists(string? path)
    {
        return Directory.Exists(path);
    }

    public IDirectoryInfo CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
        return new DirectoryInfoImplementation(path);
    }

    public IEnumerable<string> GetDirectories(string directory)
    {
        return Directory.GetDirectories(directory);
    }

    public IEnumerable<string> GetFiles(string directory)
    {
        return Directory.GetFiles(directory);
    }

    public void Delete(string path, bool recursive)
    {
        Directory.Delete(path, recursive);
    }
}