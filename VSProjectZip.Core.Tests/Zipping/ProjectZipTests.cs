using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

namespace VSProjectZip.Core.Tests.Zipping;

[TestFixture]
public class ProjectZipTests
{
#pragma warning disable CS8618
    private ProjectZip _projectZip;
    private Mock<IDirectoryCopier> _directoryCopierMock;
    private Mock<ITemporaryLocation> _temporaryLocationMock;
    private Mock<IFile> _fileMock;
    private Mock<IZipFile> _zipFileMock;
#pragma warning restore CS8618

    [SetUp]
    public void Setup()
    {
        _directoryCopierMock = new Mock<IDirectoryCopier>();
        _temporaryLocationMock = new Mock<ITemporaryLocation>();
        _fileMock = new Mock<IFile>();
        _zipFileMock = new Mock<IZipFile>();
        _projectZip = new ProjectZip(_directoryCopierMock.Object, _temporaryLocationMock.Object,
            _fileMock.Object, _zipFileMock.Object);
    }

    [Test]
    public void ZipDirectory_CallsToZipFile_s_ZipDirectory()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        string testTemp = "C:/testTemp";
        _temporaryLocationMock.Setup(location => location.TemporaryPath).Returns(testTemp);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _zipFileMock.Verify(zipFile => zipFile.CreateFromDirectory(testTemp, destinationArchive), Times.Once);
    }

    [Test]
    public void ZipDirectory_CallsTemporaryLocationAndDirectoryCopier()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        string testTemp = "C:/testTemp";
        _temporaryLocationMock.Setup(location => location.TemporaryPath).Returns(testTemp);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _temporaryLocationMock.Verify(location => location.CreateIfDoesNotExist(), Times.Once);
        _directoryCopierMock.Verify(copier => copier.CopyDirectory(testPath, testTemp), Times.Once);
        _temporaryLocationMock.Verify(location => location.DeleteIfExists(), Times.Once);
    }

    [Test]
    public void ZipDirectory_DeletesPreviousArchive_IfItExists()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        _fileMock.Setup(file => file.Exists(destinationArchive)).Returns(true);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _fileMock.Verify(file => file.Delete(destinationArchive), Times.Once);
    }
    
    [Test]
    public void ZipDirectory_DoesntTryToDeletePreviousArchive_IfItDoesNotExist()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        _fileMock.Setup(file => file.Exists(destinationArchive)).Returns(false);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _fileMock.Verify(file => file.Delete(destinationArchive), Times.Never);
    }
}