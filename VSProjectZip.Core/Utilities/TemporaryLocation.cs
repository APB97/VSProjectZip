using System.Diagnostics;
using VSProjectZip.Core.FileManagement;

namespace VSProjectZip.Core.Utilities
{
    public class TemporaryLocation : IDisposable
    {
        public const string TempLocationName = "temp";
        private readonly string _temporaryPath;
        private readonly IDirectoryCopier _copyUtility;
        private readonly IDirectory _directory;
        private bool disposedValue;

        public string TemporaryPath => _temporaryPath;

        public TemporaryLocation(string rootPath, IDirectoryCopier copyUtility, string directoryName, IDirectory directory, IPath path)
        {
            Debug.Assert(_directory != null, nameof(_directory) + " != null");

            _temporaryPath = path.Combine(rootPath, TempLocationName, directoryName);
            if (!_directory.Exists(_temporaryPath))
                _directory.CreateDirectory(_temporaryPath);
            _copyUtility = copyUtility;
            _directory = directory;
        }

        public void ReceiveDirectoryCopy(string directory)
        {
            _copyUtility.CopyDirectory(directory, _temporaryPath);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    if (_directory.Exists(_temporaryPath))
                    {
                        _directory.Delete(_temporaryPath, true);
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~TemporaryLocation()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
