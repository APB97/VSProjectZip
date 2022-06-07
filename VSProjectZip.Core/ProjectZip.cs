using System.IO.Compression;

namespace VSProjectZip.Core
{
    public class ProjectZip : IDirectoryZip
    {
        public void ZipDirectory(string path)
        {
            var rootPathName = new DirectoryInfo(path).Name;
            if (rootPathName is null) return;

            string zipName = $"{rootPathName}.zip";
            var zipFile = Path.Combine(path, zipName);
            SkipNamesCopyUtility skipNamesCopy = new(additionalFilesToSkip: new HashSet<string>() { zipName });

            using var temp = new TemporaryLocation(AppContext.BaseDirectory, skipNamesCopy, rootPathName);
            
            temp.RecieveDirectoryCopy(path);

            if (File.Exists(zipFile))
            {
                File.Delete(zipFile);
            }
            ZipFile.CreateFromDirectory(temp.TemporaryPath, zipFile);
        }
    }
}