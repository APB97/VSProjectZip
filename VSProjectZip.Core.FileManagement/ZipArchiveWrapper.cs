using System.IO.Compression;

namespace VSProjectZip.Core.FileManagement;

public class ZipArchiveWrapper : IAsyncDisposable, IZipArchiveWrapper
{
    private ZipArchive? archive;

    public async Task<IZipArchiveWrapper> OpenAsync(string outputPath)
    {
        archive = await ZipFile.OpenAsync(outputPath, ZipArchiveMode.Create);
        return this;
    }

    public async Task<ZipArchiveEntry?> CreateEntryFromFileAsync(string file, string entryName)
    {
        if (archive is null)
        {
            return null;
        }

        return await archive.CreateEntryFromFileAsync(file, entryName);
    }

    public async ValueTask DisposeAsync()
    {
        if (archive is not null)
        {
            await archive.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}
