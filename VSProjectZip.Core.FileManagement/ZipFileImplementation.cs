using System.IO.Compression;

namespace VSProjectZip.Core.FileManagement;

public class ZipFileImplementation : IZipFile
{
    public void CreateFromDirectory(string path, string destinationFileName)
    {
        ZipFile.CreateFromDirectory(path, destinationFileName);
    }
}