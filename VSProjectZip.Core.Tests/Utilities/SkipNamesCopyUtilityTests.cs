using FakeItEasy;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Utilities;

public class SkipNamesCopyUtilityTests
{
    private readonly Fake<IDirectory> _directoryMock;
    private readonly Fake<IFile> _fileMock;
    private readonly Fake<IPath> _pathMock;
    private readonly SkipNamesCopyUtility _skipNamesCopyUtility;

    public SkipNamesCopyUtilityTests()
    {
        _directoryMock = new Fake<IDirectory>();
        _fileMock = new Fake<IFile>();
        _pathMock = new Fake<IPath>();
        var fileSystemMock = new Fake<IFileSystem>();
        fileSystemMock.CallsTo(system => system.Directory).Returns(_directoryMock.FakedObject);
        fileSystemMock.CallsTo(system => system.File).Returns(_fileMock.FakedObject);
        fileSystemMock.CallsTo(system => system.Path).Returns(_pathMock.FakedObject);
        IFileSystem fileSystem = fileSystemMock.FakedObject;

        _skipNamesCopyUtility = new SkipNamesCopyUtility(fileSystem);
    }

    [Fact]
    public void AfterCreation_SkipTheseFilesIsNotEmpty()
    {
        Assert.NotEmpty(_skipNamesCopyUtility.SkipTheseFiles);
    }
    
    [Fact]
    public void ClearFiles_EmptiesSkipTheseFilesSet()
    {
        _skipNamesCopyUtility.ClearFiles();
        
        Assert.Empty(_skipNamesCopyUtility.SkipTheseFiles);
    }
    
    [Fact]
    public void AfterCreation_SkipTheseDirectoriesIsNotEmpty()
    {
        Assert.NotEmpty(_skipNamesCopyUtility.SkipTheseDirectories);
    }
    
    [Fact]
    public void ClearFiles_EmptiesSkipTheseDirectoriesSet()
    {
        _skipNamesCopyUtility.ClearDirectories();
        
        Assert.Empty(_skipNamesCopyUtility.SkipTheseDirectories);
    }

    [Fact]
    public void AddFiles_AddsFilesToSkippedFilesSet()
    {
        string file1 = "skipThisOne";
        string file2 = "skipAnotherOne";
        var files = new[] { file1, file2 };

        _skipNamesCopyUtility.AddFiles(files);

        Assert.Contains(file1, _skipNamesCopyUtility.SkipTheseFiles);
        Assert.Contains(file2, _skipNamesCopyUtility.SkipTheseFiles);
    }

    [Fact]
    public void AddDirectories_AddsDirectoriesToSkippedDirectoriesSet()
    {
        string directory1 = "skipThisDirectory";
        string directory2 = "skipThatDirectory";
        var directories = new[] { directory1, directory2 };
        
        _skipNamesCopyUtility.AddDirectories(directories);
        
        Assert.Contains(directory1, _skipNamesCopyUtility.SkipTheseDirectories);
        Assert.Contains(directory2, _skipNamesCopyUtility.SkipTheseDirectories);
    }

    [Fact]
    public void CopyDirectory_SkipsFileWhenItsNameIsInSkippedFilesSet()
    {
        string fakeDirectory = "FakeDirectory";
        string source = $"C:/{fakeDirectory}";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.CallsTo(dir => dir.Exists(destination)).Returns(true);
        _directoryMock.CallsTo(dir => dir.Exists(source)).Returns(true);
        
        string fileName = "fakeFile.txt";
        string fullFilePath = $"{source}/{fileName}";
        string destinationFileName = $"{destination}/{fileName}";
        _directoryMock.CallsTo(directory => directory.GetFiles(source)).Returns([fullFilePath]);
        
        _pathMock.CallsTo(path => path.GetFileName(fullFilePath)).Returns(fileName);
        _pathMock.CallsTo(path => path.GetRelativePath(source, fullFilePath)).Returns(fileName);
        _pathMock.CallsTo(path => path.GetDirectoryName(destinationFileName)).Returns(destination);
        _pathMock.CallsTo(path => path.GetDirectoryName(source)).Returns(fakeDirectory);
        _pathMock.CallsTo(path => path.Combine(destination, fileName)).Returns(destinationFileName);
        
        _skipNamesCopyUtility.ClearFiles();
        _skipNamesCopyUtility.AddFiles([fileName]);
        
        _skipNamesCopyUtility.CopyDirectory(source, destination);
        
        _fileMock.CallsTo(file => file.Copy(fullFilePath, destinationFileName, true)).MustNotHaveHappened();
    }

    [Fact]
    public void CopyDirectory_SkipsFileInSkippedDirectory()
    {
        string fakeDirectory = "FakeDirectory";
        string source = $"C:/{fakeDirectory}";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.CallsTo(dir => dir.Exists(destination)).Returns(true);
        _directoryMock.CallsTo(dir => dir.Exists(source)).Returns(true);

        string directoryName = "fakeSubDirectory";
        string fileName = "fakeFile.txt";
        string relativePath = $"{directoryName}/{fileName}";
        string fullFilePath = $"{source}/{relativePath}";
        string destinationFileName = $"{destination}/{relativePath}";
        string subdirectoryFullPath = $"{source}/{directoryName}";
        _directoryMock.CallsTo(directory => directory.GetFiles(source)).Returns([]);
        _directoryMock.CallsTo(directory => directory.GetDirectories(source)).Returns([subdirectoryFullPath]);
        _directoryMock.CallsTo(directory => directory.GetFiles(subdirectoryFullPath)).Returns([fullFilePath]);

        _skipNamesCopyUtility.ClearDirectories();
        _skipNamesCopyUtility.AddDirectories([directoryName]);
        
        _skipNamesCopyUtility.CopyDirectory(source, destination);
        
        _fileMock.CallsTo(file => file.Copy(fullFilePath, destinationFileName, true)).MustNotHaveHappened();
    }
}