namespace VSProjectZip.Core.FileManagement;

public class PathImplementation : IPath
{
    public string GetFileName(string file)
    {
        return Path.GetFileName(file);
    }

    public string GetRelativePath(string source, string file)
    {
        return Path.GetRelativePath(source, file);
    }

    public string Combine(string destination, string relativePath)
    {
        return Path.Combine(destination, relativePath);
    }

    public string Combine(params string[] paths)
    {
        return Path.Combine(paths);
    }

    public string? GetDirectoryName(string destinationFileName)
    {
        return Path.GetDirectoryName(destinationFileName);
    }
}