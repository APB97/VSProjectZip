using FakeItEasy;
using System.IO.Compression;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

namespace VSProjectZip.Core.Tests.Zipping;

public class ProjectZipTests
{
    private readonly Fake<IDirectoryEnumerator> _directoryCopierMock;
    private readonly Fake<IFile> _fileMock;
    private readonly Fake<IZipFile> _zipFileMock;
    private readonly Fake<IDirectory> _directoryMock;
    private readonly string testPath = "C:/testDirectory";
    private readonly string destinationArchive = "C:/test/test.zip";

    public ProjectZipTests()
    {
        _directoryCopierMock = new Fake<IDirectoryEnumerator>();
        _fileMock = new Fake<IFile>();
        _zipFileMock = new Fake<IZipFile>();
        _directoryMock = new Fake<IDirectory>();
        var archiveMock = new Fake<IZipArchiveWrapper>();
        archiveMock.CallsTo(a => a.CreateEntryFromFileAsync(A<string>.Ignored, A<string>.Ignored)).Returns((ZipArchiveEntry?)null);
        _zipFileMock.CallsTo(zf => zf.CreateAsync(A<string>.Ignored)).Returns(archiveMock.FakedObject);
    }

    [Fact]
    public async Task ZipDirectory_CallsToZipFile_s_ZipDirectoryAsync()
    {
        var projectZip = new ProjectZip(_directoryCopierMock.FakedObject, _zipFileMock.FakedObject, _fileMock.FakedObject, _directoryMock.FakedObject);
        
        await projectZip.ZipDirectoryAsync(testPath, destinationArchive);
        
        _zipFileMock.CallsTo(zipFile => zipFile.CreateAsync(destinationArchive)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task ZipDirectory_DoesNotDelete()
    {
        _fileMock.CallsTo(f => f.Exists(destinationArchive)).Returns(true);
        var projectZip = new ProjectZip(_directoryCopierMock.FakedObject, _zipFileMock.FakedObject, _fileMock.FakedObject, _directoryMock.FakedObject, false);

        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () => await projectZip.ZipDirectoryAsync(testPath, destinationArchive));
        _fileMock.CallsTo(f => f.Delete(destinationArchive)).MustNotHaveHappened();
    }

    [Fact]
    public async Task ZipDirectory_DoesDelete_WhenForceIsSet()
    {
        _fileMock.CallsTo(f => f.Exists(destinationArchive)).Returns(true);
        var projectZip = new ProjectZip(_directoryCopierMock.FakedObject, _zipFileMock.FakedObject, _fileMock.FakedObject, _directoryMock.FakedObject, true);

        await projectZip.ZipDirectoryAsync(testPath, destinationArchive);
        _fileMock.CallsTo(f => f.Delete(destinationArchive)).MustHaveHappenedOnceExactly();
    }
}
