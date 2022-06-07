﻿namespace VSProjectZip.Core.Utilities
{
    public class CopyUtility : IDirectoryCopier
    {
        public void CopyDirectory(string source, string destination)
        {
            if (Directory.Exists(source))
            {
                CopyDirectoryInternal(source, source, destination);
            }
        }

        private void CopyDirectoryInternal(string source, string directory, string destination)
        {
            if (ShouldSkipDirectory(new DirectoryInfo(directory).Name)) return;
            EnsureDestinationExists(destination);
            CopySubdirectories(source, directory, destination);
            CopyFiles(source, directory, destination);
        }

        private void EnsureDestinationExists(string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
        }

        private void CopySubdirectories(string source, string directory, string destination)
        {
            foreach (var subdirectory in Directory.GetDirectories(directory))
            {
                CopySubdirectory(source, destination, subdirectory);
            }
        }

        private void CopySubdirectory(string source, string destination, string subdirectory)
        {
            string? directoryName = new DirectoryInfo(subdirectory).Name;
            if (directoryName is not null)
            {
                CopyDirectoryInternal(source, subdirectory, destination);
            }
        }

        protected virtual bool ShouldSkipDirectory(string directoryName)
        {
            return false;
        }

        private void CopyFiles(string source, string directory, string destination)
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                CopyFile(source, destination, file);
            }
        }

        private void CopyFile(string source, string destination, string file)
        {
            string? fileName = Path.GetFileName(file);
            if (fileName is not null && !ShouldSkipFile(fileName))
            {
                string relativePath = Path.GetRelativePath(source, file);
                string destinationFileName = Path.Combine(destination, relativePath);
                string? destinationDirectoryName = Path.GetDirectoryName(destinationFileName);
                if (destinationDirectoryName is null) return;
                EnsureDestinationExists(destinationDirectoryName);
                File.Copy(file, destinationFileName, true);
            }
        }

        protected virtual bool ShouldSkipFile(string fileName)
        {
            return false;
        }
    }
}