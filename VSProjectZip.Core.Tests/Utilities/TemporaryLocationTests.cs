using FakeItEasy;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Utilities;

public class TemporaryLocationTests
{
    private readonly Fake<IDirectory> _directoryMock;
    private readonly TemporaryLocation _temporaryLocation;
    private const string TestPath = "C:/testTemp";

    public TemporaryLocationTests()
    {
        _directoryMock = new Fake<IDirectory>();
        _temporaryLocation = new TemporaryLocation(_directoryMock.FakedObject, TestPath);
    }
    
    [Fact]
    public void CreateIfDoesNotExist_CallsCreateDirectory_WhenItDoesNotExist()
    {
        _directoryMock.CallsTo(directory => directory.Exists(TestPath)).Returns(false);
        
        _temporaryLocation.CreateIfDoesNotExist();
        
        _directoryMock.CallsTo(directory => directory.CreateDirectory(TestPath)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void DeleteIfExists_CallsDeleteOnDirectory_WhenItExists()
    {
        _directoryMock.CallsTo(directory => directory.Exists(TestPath)).Returns(true);
        
        _temporaryLocation.DeleteIfExists();
        
        _directoryMock.CallsTo(directory => directory.Delete(TestPath, true)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void GetTemporaryPath_CallsToPathCombineWithCorrectParameters()
    {
        var pathMock = new Fake<IPath>();
        const string baseDirectory = "C:/Apps/ProjectZip";
        const string testSubdirectoryName = "TestDirectoryToZip";

        _ = TemporaryLocation.GetTemporaryPath(pathMock.FakedObject, baseDirectory, testSubdirectoryName);
        
        pathMock.CallsTo(path => path.Combine(baseDirectory, TemporaryLocation.TempLocationName, testSubdirectoryName))
            .MustHaveHappenedOnceExactly();
    }
}