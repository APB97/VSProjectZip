namespace VSProjectZip.Core.FileManagement;

public class FileSystem(IDirectory directory, IFile file, IPath path) : IFileSystem
{
    public IDirectory Directory { get; } = directory;
    public IFile File { get; } = file;
    public IPath Path { get; } = path;
}
