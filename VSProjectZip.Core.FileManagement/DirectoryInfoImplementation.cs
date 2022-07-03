namespace VSProjectZip.Core.FileManagement;

public class DirectoryInfoImplementation : IDirectoryInfo
{
    private readonly IDirectory _directory;

    public DirectoryInfoImplementation(IDirectory directory, string directoryPath)
    {
        _directory = directory;
        Name = new DirectoryInfo(directoryPath).Name;
    }

    public string Name { get; }
}