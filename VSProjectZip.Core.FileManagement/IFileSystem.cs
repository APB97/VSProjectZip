namespace VSProjectZip.Core.FileManagement;

public interface IFileSystem
{
    IDirectory Directory { get; }
    IFile File { get; }
    IPath Path { get; }
    ITemporaryLocation Temporary { get; }    
}