namespace VSProjectZip.Core.FileManagement;

public interface IZipFile
{
    Task<IZipArchiveWrapper> CreateAsync(string outputPath);
}
