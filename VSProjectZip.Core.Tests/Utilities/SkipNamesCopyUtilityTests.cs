using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Utilities;

[TestFixture]
public class SkipNamesCopyUtilityTests
{
    // disable warning about uninitialized field(s)
#pragma warning disable CS8618
    private Mock<IDirectory> _directoryMock;
    private Mock<IFile> _fileMock;
    private Mock<IPath> _pathMock;
    private SkipNamesCopyUtility _skipNamesCopyUtility;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _directoryMock = new Mock<IDirectory>();
        _fileMock = new Mock<IFile>();
        _pathMock = new Mock<IPath>();
        Mock<IFileSystem> fileSystemMock = new Mock<IFileSystem>();
        fileSystemMock.SetupGet(system => system.Directory).Returns(_directoryMock.Object);
        fileSystemMock.SetupGet(system => system.File).Returns(_fileMock.Object);
        fileSystemMock.SetupGet(system => system.Path).Returns(_pathMock.Object);
        IFileSystem fileSystem = fileSystemMock.Object;

        _skipNamesCopyUtility = new SkipNamesCopyUtility(fileSystem);
    }

    [Test]
    public void AfterCreation_SkipTheseFilesIsNotEmpty()
    {
        Assert.That(_skipNamesCopyUtility.SkipTheseFiles, Is.Not.Empty);
    }
    
    [Test]
    public void ClearFiles_EmptiesSkipTheseFilesSet()
    {
        _skipNamesCopyUtility.ClearFiles();
        
        Assert.That(_skipNamesCopyUtility.SkipTheseFiles, Is.Empty);
    }
    
    [Test]
    public void AfterCreation_SkipTheseDirectoriesIsNotEmpty()
    {
        Assert.That(_skipNamesCopyUtility.SkipTheseDirectories, Is.Not.Empty);
    }
    
    [Test]
    public void ClearFiles_EmptiesSkipTheseDirectoriesSet()
    {
        _skipNamesCopyUtility.ClearDirectories();
        
        Assert.That(_skipNamesCopyUtility.SkipTheseDirectories, Is.Empty);
    }

    [Test]
    public void AddFiles_AddsFilesToSkippedFilesSet()
    {
        string file1 = "skipThisOne";
        string file2 = "skipAnotherOne";
        var files = new[] { file1, file2 };

        _skipNamesCopyUtility.AddFiles(files);
        
        Assert.That(_skipNamesCopyUtility.SkipTheseFiles, Contains.Item(file1).And.Contains(file2));
    }

    [Test]
    public void AddDirectories_AddsDirectoriesToSkippedDirectoriesSet()
    {
        string directory1 = "skipThisDirectory";
        string directory2 = "skipThatDirectory";
        var directories = new[] { directory1, directory2 };
        
        _skipNamesCopyUtility.AddDirectories(directories);
        
        Assert.That(_skipNamesCopyUtility.SkipTheseDirectories, Contains.Item(directory1).And.Contains(directory2));
    }

    [Test]
    public void CopyDirectory_SkipsFileWhenItsNameIsInSkippedFilesSet()
    {
        string fakeDirectory = "FakeDirectory";
        string source = $"C:/{fakeDirectory}";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.Setup(dir => dir.Exists(destination)).Returns(true);
        _directoryMock.Setup(dir => dir.Exists(source)).Returns(true);
        
        string fileName = "fakeFile.txt";
        string fullFilePath = $"{source}/{fileName}";
        string destinationFileName = $"{destination}/{fileName}";
        _directoryMock.Setup(directory => directory.GetFiles(source)).Returns(new[] { fullFilePath });
        
        _pathMock.Setup(path => path.GetFileName(fullFilePath)).Returns(fileName);
        _pathMock.Setup(path => path.GetRelativePath(source, fullFilePath)).Returns(fileName);
        _pathMock.Setup(path => path.GetDirectoryName(destinationFileName)).Returns(destination);
        _pathMock.Setup(path => path.GetDirectoryName(source)).Returns(fakeDirectory);
        _pathMock.Setup(path => path.Combine(destination, fileName)).Returns(destinationFileName);
        
        _skipNamesCopyUtility.ClearFiles();
        _skipNamesCopyUtility.AddFiles(new []{ fileName });
        
        _skipNamesCopyUtility.CopyDirectory(source, destination);
        
        _fileMock.Verify(file => file.Copy(fullFilePath, destinationFileName, true), Times.Never);
    }

    [Test]
    public void CopyDirectory_SkipsFileInSkippedDirectory()
    {
        string fakeDirectory = "FakeDirectory";
        string source = $"C:/{fakeDirectory}";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.Setup(dir => dir.Exists(destination)).Returns(true);
        _directoryMock.Setup(dir => dir.Exists(source)).Returns(true);

        string directoryName = "fakeSubDirectory";
        string fileName = "fakeFile.txt";
        string relativePath = $"{directoryName}/{fileName}";
        string fullFilePath = $"{source}/{relativePath}";
        string destinationFileName = $"{destination}/{relativePath}";
        string subdirectoryFullPath = $"{source}/{directoryName}";
        _directoryMock.Setup(directory => directory.GetFiles(source)).Returns(Array.Empty<string>());
        _directoryMock.Setup(directory => directory.GetDirectories(source)).Returns(new[] { subdirectoryFullPath });
        _directoryMock.Setup(directory => directory.GetFiles(subdirectoryFullPath)).Returns(new[] { fullFilePath });
        
        _pathMock.Setup(path => path.GetFileName(fullFilePath)).Returns(fileName);
        _pathMock.Setup(path => path.GetRelativePath(source, fullFilePath)).Returns(relativePath);
        _pathMock.Setup(path => path.GetDirectoryName(destinationFileName)).Returns(directoryName);
        _pathMock.Setup(path => path.GetDirectoryName(source)).Returns(fakeDirectory);
        _pathMock.Setup(path => path.Combine(destination, relativePath)).Returns(destinationFileName);
        
        _skipNamesCopyUtility.ClearDirectories();
        _skipNamesCopyUtility.AddDirectories(new[] { directoryName });
        
        _skipNamesCopyUtility.CopyDirectory(source, destination);
        
        _fileMock.Verify(file => file.Copy(fullFilePath, destinationFileName, true), Times.Never);
    }
}