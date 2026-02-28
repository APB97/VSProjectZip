using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Zipping
{
    public class ProjectZip(IDirectoryEnumerator directoryCopier, IZipFile zipFile, IFile file, IDirectory directory, bool force = false) : IDirectoryZip
    {
        private readonly IDirectoryEnumerator _directoryCopier = directoryCopier;
        private readonly IZipFile zipFile = zipFile;
        private readonly IFile file = file;
        private readonly IDirectory directory = directory;

        public async Task ZipDirectoryAsync(string path, string outputZipPath)
        {
            if (file.Exists(outputZipPath))
            {
                if (force)
                {
                    file.Delete(outputZipPath);
                }
                else
                {
                    throw new InvalidOperationException("File exists and force parameter is not set");
                }
            }

            await using var zip = await zipFile.CreateAsync(outputZipPath);
            await TraverseAndZip(path, path, zip);
        }

        private async Task TraverseAndZip(string rootPath, string path, IZipArchiveWrapper zip)
        {
            foreach (var entry in _directoryCopier.FilterOutSkippedItems(_directoryCopier.EnumerateEntries(path)))
            {
                if (directory.Exists(entry))
                {
                    await TraverseAndZip(rootPath, entry, zip);
                }
                else
                {
                    string relativePath = Path.GetRelativePath(rootPath, entry);
                    await zip.CreateEntryFromFileAsync(entry, relativePath);
                }
            }
        }
    }
}