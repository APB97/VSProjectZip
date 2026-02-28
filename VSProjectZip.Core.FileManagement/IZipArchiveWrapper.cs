using System.IO.Compression;

namespace VSProjectZip.Core.FileManagement
{
    public interface IZipArchiveWrapper
    {
        Task<ZipArchiveEntry?> CreateEntryFromFileAsync(string file, string entryName);
        ValueTask DisposeAsync();
        Task<IZipArchiveWrapper> OpenAsync(string outputPath);
    }
}