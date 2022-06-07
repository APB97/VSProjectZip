using System.IO.Compression;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Zipping
{
    public class ProjectZip : IDirectoryZip
    {
        private readonly IDirectoryCopier _directoryCopier;

        public ProjectZip(IDirectoryCopier directoryCopier)
        {
            _directoryCopier = directoryCopier;
        }

        public void ZipDirectory(string path, string outputZipPath)
        {
            var rootPathName = new DirectoryInfo(path).Name;
            if (rootPathName is null) return;

            string zipName = $"{rootPathName}.zip";
            var zipFile = Path.Combine(outputZipPath, zipName);

            using var temp = new TemporaryLocation(AppContext.BaseDirectory, _directoryCopier, rootPathName);

            temp.RecieveDirectoryCopy(path);

            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
            ZipFile.CreateFromDirectory(temp.TemporaryPath, zipFile);
        }
    }
}