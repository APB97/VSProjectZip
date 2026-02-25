using FakeItEasy;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Utilities;

public class CopyUtilityTests
{
    private readonly CopyUtility _copier;
    private readonly Fake<IDirectory> _directoryMock;
    private readonly Fake<IFile> _fileMock;
    private readonly Fake<IPath> _pathMock;

    public CopyUtilityTests()
    {
        _directoryMock = new Fake<IDirectory>();
        _fileMock = new Fake<IFile>();
        _pathMock = new Fake<IPath>();
        var fileSystemMock = new Fake<IFileSystem>();
        fileSystemMock.CallsTo(system => system.Directory).Returns(_directoryMock.FakedObject);
        fileSystemMock.CallsTo(system => system.File).Returns(_fileMock.FakedObject);
        fileSystemMock.CallsTo(system => system.Path).Returns(_pathMock.FakedObject);
        IFileSystem fileSystem = fileSystemMock.FakedObject;
        
        _copier = new CopyUtility(fileSystem);
    }

    [Fact]
    public void CopyDirectory_CreatesDirectory_WhenDestinationDoesntExist_ButSourceDoes()
    {
        const string fakeDirectory = "FakeDirectory";
        string source = $"C:/{fakeDirectory}";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.CallsTo(dir => dir.Exists(destination)).Returns(false);
        _directoryMock.CallsTo(dir => dir.Exists(source)).Returns(true);

        _pathMock.CallsTo(path => path.GetDirectoryName(source)).Returns(fakeDirectory);

        _copier.CopyDirectory(source, destination);
        
        _directoryMock.CallsTo(dir => dir.CreateDirectory(destination)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void CopyDirectory_DoesntCreateDirectory_WhenDestinationAndSourceExist()
    {
        string source = "C:/FakeDirectory";
        string destination = "C:/AnotherFakeDirectory";
        _directoryMock.CallsTo(dir => dir.Exists(destination)).Returns(true);
        _directoryMock.CallsTo(dir => dir.Exists(source)).Returns(true);

        _copier.CopyDirectory(source, destination);
        
        _directoryMock.CallsTo(dir => dir.CreateDirectory(destination)).MustNotHaveHappened();
    }

    [Fact]
    public void CopyDirectory_CopiesFileAtDirectoryRoot()
    {
        const string fakeDirectory = "FakeDirectory";
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
        
        _copier.CopyDirectory(source, destination);
        
        _fileMock.CallsTo(file => file.Copy(fullFilePath, destinationFileName, true)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void CopyDirectory_CopiesFileAtSubdirectory()
    {
        const string fakeDirectory = "FakeDirectory";
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
        
        _pathMock.CallsTo(path => path.GetFileName(fullFilePath)).Returns(fileName);
        _pathMock.CallsTo(path => path.GetRelativePath(source, fullFilePath)).Returns(relativePath);
        _pathMock.CallsTo(path => path.GetDirectoryName(destinationFileName)).Returns(directoryName);
        _pathMock.CallsTo(path => path.GetDirectoryName(source)).Returns(fakeDirectory);
        _pathMock.CallsTo(path => path.GetDirectoryName(subdirectoryFullPath)).Returns(directoryName);
        _pathMock.CallsTo(path => path.Combine(destination, relativePath)).Returns(destinationFileName);
        
        _copier.CopyDirectory(source, destination);
        
        _fileMock.CallsTo(file => file.Copy(fullFilePath, destinationFileName, true)).MustHaveHappenedOnceExactly();
    }
}