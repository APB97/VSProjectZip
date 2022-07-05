using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests;

[TestFixture]
public class SkipNamesCopyUtilityTests
{
    private Mock<IDirectory> directoryMock;
    private Mock<IFile> fileMock;
    private Mock<IPath> pathMock;
    private SkipNamesCopyUtility skipNamesCopyUtility;

    [SetUp]
    public void Setup()
    {
        directoryMock = new Mock<IDirectory>();
        fileMock = new Mock<IFile>();
        pathMock = new Mock<IPath>();

        skipNamesCopyUtility = new SkipNamesCopyUtility(directoryMock.Object, fileMock.Object, pathMock.Object);
    }

    [Test]
    public void AfterCreation_SkipTheseFilesIsNotEmpty()
    {
        Assert.That(skipNamesCopyUtility.SkipTheseFiles, Is.Not.Empty);
    }
    
    [Test]
    public void ClearFiles_EmptiesSkipTheseFilesSet()
    {
        skipNamesCopyUtility.ClearFiles();
        
        Assert.That(skipNamesCopyUtility.SkipTheseFiles, Is.Empty);
    }
    
    [Test]
    public void AfterCreation_SkipTheseDirectoriesIsNotEmpty()
    {
        Assert.That(skipNamesCopyUtility.SkipTheseDirectories, Is.Not.Empty);
    }
    
    [Test]
    public void ClearFiles_EmptiesSkipTheseDirectoriesSet()
    {
        skipNamesCopyUtility.ClearDirectories();
        
        Assert.That(skipNamesCopyUtility.SkipTheseDirectories, Is.Empty);
    }

    [Test]
    public void AddFiles_AddsFilesToSkippedFilesSet()
    {
        string file1 = "skipThisOne";
        string file2 = "skipAnotherOne";
        var files = new[] { file1, file2 };

        skipNamesCopyUtility.AddFiles(files);
        
        Assert.That(skipNamesCopyUtility.SkipTheseFiles, Contains.Item(file1).And.Contains(file2));
    }

    [Test]
    public void AddDirectories_AddsDirectoriesToSkippedDirectoriesSet()
    {
        string directory1 = "skipThisDirectory";
        string directory2 = "skipThatDirectory";
        var directories = new[] { directory1, directory2 };
        
        skipNamesCopyUtility.AddDirectories(directories);
        
        Assert.That(skipNamesCopyUtility.SkipTheseDirectories, Contains.Item(directory1).And.Contains(directory2));
    }
}