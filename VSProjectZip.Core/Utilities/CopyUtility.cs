﻿using System.Diagnostics;
using VSProjectZip.Core.FileManagement;

namespace VSProjectZip.Core.Utilities
{
    public class CopyUtility : IDirectoryCopier
    {
        private readonly IDirectory _directory;
        private readonly IFile _file;
        private readonly IPath _path;

        public CopyUtility(IFileSystem fileSystem)
        {
            _directory = fileSystem.Directory;
            _file = fileSystem.File;
            _path = fileSystem.Path;
        }
        
        public void CopyDirectory(string source, string destination)
        {
            if (_directory.Exists(source))
            {
                CopyDirectoryInternal(source, source, destination);
            }
        }

        private void CopyDirectoryInternal(string source, string directoryPath, string destination)
        {
            string? directoryName = _path.GetDirectoryName(directoryPath);
            if (directoryName is null || ShouldSkipDirectory(directoryName)) return;
            EnsureDestinationExists(destination);
            CopySubdirectories(source, directoryPath, destination);
            CopyFiles(source, directoryPath, destination);
        }

        private void EnsureDestinationExists(string destination)
        {
            if (!_directory.Exists(destination))
            {
                _directory.CreateDirectory(destination);
            }
        }

        private void CopySubdirectories(string source, string directory, string destination)
        {
            foreach (var subdirectory in _directory.GetDirectories(directory))
            {
                CopySubdirectory(source, destination, subdirectory);
            }
        }

        private void CopySubdirectory(string source, string destination, string subdirectory)
        {
            CopyDirectoryInternal(source, subdirectory, destination);
        }

        protected virtual bool ShouldSkipDirectory(string directoryName)
        {
            return false;
        }

        private void CopyFiles(string source, string directory, string destination)
        {
            foreach (var file in _directory.GetFiles(directory))
            {
                CopyFile(source, destination, file);
            }
        }

        private void CopyFile(string source, string destination, string file)
        {
            string? fileName = _path.GetFileName(file);
            if (fileName is not null && !ShouldSkipFile(fileName))
            {
                string relativePath = _path.GetRelativePath(source, file);
                string destinationFileName = _path.Combine(destination, relativePath);
                string? destinationDirectoryName = _path.GetDirectoryName(destinationFileName);
                Debug.Assert(destinationDirectoryName != null, nameof(destinationDirectoryName) + " != null");
                EnsureDestinationExists(destinationDirectoryName);
                _file.Copy(file, destinationFileName, true);
            }
        }

        protected virtual bool ShouldSkipFile(string fileName)
        {
            return false;
        }
    }
}
