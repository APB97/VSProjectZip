using Moq;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests;

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

        _skipNamesCopyUtility = new SkipNamesCopyUtility(_directoryMock.Object, _fileMock.Object, _pathMock.Object);
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
}