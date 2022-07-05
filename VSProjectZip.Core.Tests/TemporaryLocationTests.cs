using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests;

[TestFixture]
public class TemporaryLocationTests
{
#pragma warning disable CS8618
    private Mock<IDirectory> _directoryMock;
    private ITemporaryLocation _temporaryLocation;
#pragma warning restore CS8618
    private const string TestPath = "C:/testTemp";

    [SetUp]
    public void Setup()
    {
        _directoryMock = new Mock<IDirectory>();
        _temporaryLocation = new TemporaryLocation(_directoryMock.Object, TestPath);
    }
    
    [Test]
    public void CreateIfDoesNotExist_CallsCreateDirectory_WhenItDoesNotExist()
    {
        _directoryMock.Setup(directory => directory.Exists(TestPath)).Returns(false);
        
        _temporaryLocation.CreateIfDoesNotExist();
        
        _directoryMock.Verify(directory => directory.CreateDirectory(TestPath), Times.Once);
    }

    [Test]
    public void DeleteIfExists_CallsDeleteOnDirectory_WhenItExists()
    {
        _directoryMock.Setup(directory => directory.Exists(TestPath)).Returns(true);
        
        _temporaryLocation.DeleteIfExists();
        
        _directoryMock.Verify(directory => directory.Delete(TestPath, true), Times.Once);
    }
}