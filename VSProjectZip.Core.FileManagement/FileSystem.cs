namespace VSProjectZip.Core.FileManagement;

public class FileSystem : IFileSystem
{
    public IDirectory Directory { get; }
    public IFile File { get; }
    public IPath Path { get; }
    public ITemporaryLocation Temporary { get; }

    public FileSystem(IDirectory directory, IFile file, IPath path, ITemporaryLocation temporary)
    {
        Directory = directory;
        File = file;
        Path = path;
        Temporary = temporary;
    }
}