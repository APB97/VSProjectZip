namespace VSProjectZip.Core.FileManagement;

public class ZipFileImplementation : IZipFile
{
    public Task<IZipArchiveWrapper> CreateAsync(string outputPath)
    {
        var wrapper = new ZipArchiveWrapper();
        return wrapper.OpenAsync(outputPath);
    }
}
