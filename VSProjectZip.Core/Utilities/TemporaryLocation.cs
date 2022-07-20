using VSProjectZip.Core.FileManagement;

namespace VSProjectZip.Core.Utilities
{
    public class TemporaryLocation : ITemporaryLocation
    {
        public const string TempLocationName = "temp";
        private readonly IDirectory _directory;

        public string TemporaryPath { get; }

        public TemporaryLocation(IDirectory directory, string temporaryPath)
        {
            _directory = directory;
            TemporaryPath = temporaryPath;
        }

        public void CreateIfDoesNotExist()
        {
            if (!_directory.Exists(TemporaryPath))
            {
                _directory.CreateDirectory(TemporaryPath);
            }
        }

        public void DeleteIfExists()
        {
            if (_directory.Exists(TemporaryPath))
            {
                _directory.Delete(TemporaryPath, true);
            }
        }

        public static string GetTemporaryPath(IPath path, string baseDirectory, string subdirectoryName)
        {
            return path.Combine(baseDirectory, TempLocationName, subdirectoryName);
        }
    }
}
