namespace VSProjectZip.Core.FileManagement;

public class DirectoryInfoImplementation : IDirectoryInfo
{
    public DirectoryInfoImplementation(string directoryPath)
    {
        Name = new DirectoryInfo(directoryPath).Name;
    }

    public string Name { get; }
}