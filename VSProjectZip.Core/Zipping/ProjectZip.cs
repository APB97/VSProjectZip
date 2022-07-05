using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Zipping
{
    public class ProjectZip : IDirectoryZip
    {
        private readonly IDirectoryCopier _directoryCopier;
        private readonly ITemporaryLocation _temporaryLocation;
        private readonly IFile _file;
        private readonly IZipFile _zipFile;

        public ProjectZip(IDirectoryCopier directoryCopier, ITemporaryLocation temporaryLocation, IFile file, IZipFile zipFile)
        {
            _directoryCopier = directoryCopier;
            _temporaryLocation = temporaryLocation;
            _file = file;
            _zipFile = zipFile;
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
            _zipFile.CreateFromDirectory(_temporaryLocation.TemporaryPath, outputZipPath);
        }

        private void DeleteIfExist(string outputZipPath)
        {
            if (_file.Exists(outputZipPath))
            {
                _file.Delete(outputZipPath);
            }
        }
    }
}