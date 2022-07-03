using System.IO.Compression;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Zipping
{
    public class ProjectZip : IDirectoryZip
    {
        private readonly IDirectory _directory;
        private readonly IDirectoryCopier _directoryCopier;

        public ProjectZip(IDirectoryCopier directoryCopier, IDirectory directory)
        {
            _directoryCopier = directoryCopier;
            _directory = directory;
        }

        public void ZipDirectory(string path, string outputZipPath)
        {
            var rootPathName = new DirectoryInfo(path).Name;

            using var temp = new TemporaryLocation(AppContext.BaseDirectory, _directoryCopier, rootPathName, _directory, new PathImplementation());

            temp.ReceiveDirectoryCopy(path);

            if (File.Exists(outputZipPath))
            {
                File.Delete(outputZipPath);
            }
            ZipFile.CreateFromDirectory(temp.TemporaryPath, outputZipPath);
        }
    }
}