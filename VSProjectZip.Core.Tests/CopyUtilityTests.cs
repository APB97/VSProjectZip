using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests;

public class CopyUtilityTests
{
    private CopyUtility _copier;
    private Mock<IDirectory> directoryMock;
    private Mock<IFile> fileMock;
    private Mock<IPath> pathMock;

    [SetUp]
    public void Setup()
    {
        directoryMock = new Mock<IDirectory>();
        fileMock = new Mock<IFile>();
        pathMock = new Mock<IPath>();
        
        _copier = new CopyUtility(directoryMock.Object, fileMock.Object, pathMock.Object);
    }

    [Test]
    public void CopyDirectory_CreatesDirectory_WhenDestinationDoesntExist_ButSourceDoes()
    {
        string source = "C:/FakeDirectory";
        string destination = "C:/AnotherFakeDirectory";
        directoryMock.Setup(dir => dir.Exists(destination)).Returns(false);
        directoryMock.Setup(dir => dir.Exists(source)).Returns(true);

        _copier.CopyDirectory(source, destination);
        
        directoryMock.Verify(dir => dir.CreateDirectory(destination));
    }
    
    [Test]
    public void CopyDirectory_DoesntCreateDirectory_WhenDestinationAndSourceExist()
    {
        string source = "C:/FakeDirectory";
        string destination = "C:/AnotherFakeDirectory";
        directoryMock.Setup(dir => dir.Exists(destination)).Returns(true);
        directoryMock.Setup(dir => dir.Exists(source)).Returns(true);

        _copier.CopyDirectory(source, destination);
        
        directoryMock.Verify(dir => dir.CreateDirectory(destination), Times.Never);
    }

    [Test]
    public void Copy_Directory_CopiesFileAtDirectoryRoot()
    {
        string source = "C:/FakeDirectory";
        string destination = "C:/AnotherFakeDirectory";
        directoryMock.Setup(dir => dir.Exists(destination)).Returns(true);
        directoryMock.Setup(dir => dir.Exists(source)).Returns(true);
        
        string fileName = "fakeFile.txt";
        string fullFilePath = $"{source}/{fileName}";
        string destinationFileName = $"{destination}/{fileName}";
        directoryMock.Setup(directory => directory.GetFiles(source)).Returns(new[] { fullFilePath });

        pathMock.Setup(path => path.GetFileName(fullFilePath)).Returns(fileName);
        pathMock.Setup(path => path.GetRelativePath(source, fullFilePath)).Returns(fileName);
        pathMock.Setup(path => path.GetDirectoryName(destinationFileName)).Returns(destination);
        pathMock.Setup(path => path.Combine(destination, fileName)).Returns(destinationFileName);
        
        _copier.CopyDirectory(source, destination);
        
        fileMock.Verify(file => file.Copy(fullFilePath, destinationFileName, true), Times.Once);
    }
    
    [Test]
    public void Copy_Directory_CopiesFileAtSubdirectory()
    {
        string source = "C:/FakeDirectory";
        string destination = "C:/AnotherFakeDirectory";
        directoryMock.Setup(dir => dir.Exists(destination)).Returns(true);
        directoryMock.Setup(dir => dir.Exists(source)).Returns(true);

        string directoryName = "fakeSubDirectory";
        string fileName = "fakeFile.txt";
        string relativePath = $"{directoryName}/{fileName}";
        string fullFilePath = $"{source}/{relativePath}";
        string destinationFileName = $"{destination}/{relativePath}";
        directoryMock.Setup(directory => directory.GetFiles(source)).Returns(new[] { fullFilePath });

        pathMock.Setup(path => path.GetFileName(fullFilePath)).Returns(fileName);
        pathMock.Setup(path => path.GetRelativePath(source, fullFilePath)).Returns(relativePath);
        pathMock.Setup(path => path.GetDirectoryName(destinationFileName)).Returns($"{destination}/{directoryName}");
        pathMock.Setup(path => path.Combine(destination, relativePath)).Returns(destinationFileName);
        
        _copier.CopyDirectory(source, destination);
        
        fileMock.Verify(file => file.Copy(fullFilePath, destinationFileName, true), Times.Once);
    }
}