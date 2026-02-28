namespace VSProjectZip.Core.Zipping
{
    public interface IDirectoryZip
    {
        Task ZipDirectoryAsync(string path, string outputZipPath);
    }
}
