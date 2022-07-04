using System.IO.Compression;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Zipping
{
    public class ProjectZip : IDirectoryZip
    {
        private readonly IDirectoryCopier _directoryCopier;
        private readonly ITemporaryLocation _temporaryLocation;

        public ProjectZip(IDirectoryCopier directoryCopier, ITemporaryLocation temporaryLocation)
        {
            _directoryCopier = directoryCopier;
            _temporaryLocation = temporaryLocation;
        }

        public void ZipDirectory(string path, string outputZipPath)
        {
            _temporaryLocation.CreateIfDoesNotExist();
            ZipDirectoryUsingTemporaryLocation(path, outputZipPath);
            _temporaryLocation.DeleteIfExists();
        }

        private void ZipDirectoryUsingTemporaryLocation(string path, string outputZipPath)
        {
            _directoryCopier.CopyDirectory(path, _temporaryLocation.TemporaryPath);
            DeleteIfExist(outputZipPath);
            ZipFile.CreateFromDirectory(_temporaryLocation.TemporaryPath, outputZipPath);
        }

        private static void DeleteIfExist(string outputZipPath)
        {
            if (File.Exists(outputZipPath))
            {
                File.Delete(outputZipPath);
            }
        }
    }
}