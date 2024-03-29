using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Utilities;

public class CopyUtilityTests
{
    // disable warning about uninitialized field(s)
#pragma warning disable CS8618
    private CopyUtility _copier;
    private Mock<IDirectory> _directoryMock;
    private Mock<IFile> _fileMock;
    private Mock<IPath> _pathMock;
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
        
        _copier = new CopyUtility(fileSystem);
    }

    [Test]
    public void CopyDirectory_CreatesDirectory_WhenDestinationDoesntExist_ButSourceDoes()
    {
        const string fakeDirectory = "FakeDirectory";
        string source = $"C:/{fakeDirectory}";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.Setup(dir => dir.Exists(destination)).Returns(false);
        _directoryMock.Setup(dir => dir.Exists(source)).Returns(true);

        _pathMock.Setup(path => path.GetDirectoryName(source)).Returns(fakeDirectory);

        _copier.CopyDirectory(source, destination);
        
        _directoryMock.Verify(dir => dir.CreateDirectory(destination));
    }
    
    [Test]
    public void CopyDirectory_DoesntCreateDirectory_WhenDestinationAndSourceExist()
    {
        string source = "C:/FakeDirectory";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.Setup(dir => dir.Exists(destination)).Returns(true);
        _directoryMock.Setup(dir => dir.Exists(source)).Returns(true);

        _copier.CopyDirectory(source, destination);
        
        _directoryMock.Verify(dir => dir.CreateDirectory(destination), Times.Never);
    }

    [Test]
    public void CopyDirectory_CopiesFileAtDirectoryRoot()
    {
        const string fakeDirectory = "FakeDirectory";
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
        
        _copier.CopyDirectory(source, destination);
        
        _fileMock.Verify(file => file.Copy(fullFilePath, destinationFileName, true), Times.Once);
    }
    
    [Test]
    public void CopyDirectory_CopiesFileAtSubdirectory()
    {
        const string fakeDirectory = "FakeDirectory";
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
        _pathMock.Setup(path => path.GetDirectoryName(subdirectoryFullPath)).Returns(directoryName);
        _pathMock.Setup(path => path.Combine(destination, relativePath)).Returns(destinationFileName);
        
        _copier.CopyDirectory(source, destination);
        
        _fileMock.Verify(file => file.Copy(fullFilePath, destinationFileName, true), Times.Once);
    }
}