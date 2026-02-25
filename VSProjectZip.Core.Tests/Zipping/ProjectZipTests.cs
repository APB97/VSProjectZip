using FakeItEasy;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

namespace VSProjectZip.Core.Tests.Zipping;

public class ProjectZipTests
{
    private readonly ProjectZip _projectZip;
    private readonly Fake<IDirectoryCopier> _directoryCopierMock;
    private readonly Fake<ITemporaryLocation> _temporaryLocationMock;
    private readonly Fake<IFile> _fileMock;
    private readonly Fake<IZipFile> _zipFileMock;

    public ProjectZipTests()
    {
        _directoryCopierMock = new Fake<IDirectoryCopier>();
        _temporaryLocationMock = new Fake<ITemporaryLocation>();
        _fileMock = new Fake<IFile>();
        _zipFileMock = new Fake<IZipFile>();
        _projectZip = new ProjectZip(_directoryCopierMock.FakedObject, _temporaryLocationMock.FakedObject,
            _fileMock.FakedObject, _zipFileMock.FakedObject);
    }

    [Fact]
    public void ZipDirectory_CallsToZipFile_s_ZipDirectory()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        string testTemp = "C:/testTemp";
        _temporaryLocationMock.CallsTo(location => location.TemporaryPath).Returns(testTemp);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _zipFileMock.CallsTo(zipFile => zipFile.CreateFromDirectory(testTemp, destinationArchive)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ZipDirectory_CallsTemporaryLocationAndDirectoryCopier()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        string testTemp = "C:/testTemp";
        _temporaryLocationMock.CallsTo(location => location.TemporaryPath).Returns(testTemp);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _temporaryLocationMock.CallsTo(location => location.CreateIfDoesNotExist()).MustHaveHappenedOnceExactly();
        _directoryCopierMock.CallsTo(copier => copier.CopyDirectory(testPath, testTemp)).MustHaveHappenedOnceExactly();
        _temporaryLocationMock.CallsTo(location => location.DeleteIfExists()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ZipDirectory_DeletesPreviousArchive_IfItExists()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        _fileMock.CallsTo(file => file.Exists(destinationArchive)).Returns(true);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _fileMock.CallsTo(file => file.Delete(destinationArchive)).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void ZipDirectory_DoesntTryToDeletePreviousArchive_IfItDoesNotExist()
    {
        string testPath = "C:/testDirectory";
        string destinationArchive = "C:/test/test.zip";
        _fileMock.CallsTo(file => file.Exists(destinationArchive)).Returns(false);
        
        _projectZip.ZipDirectory(testPath, destinationArchive);
        
        _fileMock.CallsTo(file => file.Delete(destinationArchive)).MustNotHaveHappened();
    }
}