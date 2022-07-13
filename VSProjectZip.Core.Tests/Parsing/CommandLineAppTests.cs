using Moq;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Parsing;

[TestFixture]
public class CommandLineAppTests
{
    [Test]
    public void ReadDirectoryToZip_ReturnsNull_WhenArgumentsArrayIsEmpty()
    {
        var empty = Array.Empty<string>();
        var app = new CommandLineApp(new Mock<ILogger>().Object);

        var actualValue = app.ReadDirectoryToZipFromFirstArgument(empty);
        
        Assert.That(actualValue, Is.Null);
    }

    [Test]
    public void ReadDirectoryToZip_ReturnsDirectoryInfoWithFirstArgument_WhenArraysFirstElementIsNotNull()
    {
        string mainArgument = "C;/testDir";
        string secondArgument = "--override-skipdirs";
        var args = new string[] { mainArgument, secondArgument };
        var expectedValue = new DirectoryInfo(mainArgument);
        var app = new CommandLineApp(new Mock<ILogger>().Object);

        var actualValue = app.ReadDirectoryToZipFromFirstArgument(args);

        Assert.That(actualValue, Is.EqualTo(expectedValue));
    }

    [Test]
    public void ReadDirectoryToZip_CallsILoggerInfo_WhenArrayIsEmpty()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);

        app.ReadDirectoryToZipFromFirstArgument(Array.Empty<string>());
        
        loggerMock.Verify(logger => logger.Info(It.IsAny<string>()));
    }

    [Test]
    public void UpdateSkippedFiles_CallsClearFiles_WhenRequested()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        var skipFilesMock = new Mock<ISkipFiles>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?> { {"--override-skipfiles", null} };

        app.UpdateSkippedFiles(skipFilesMock.Object, dictionary);
        
        skipFilesMock.Verify(files => files.ClearFiles(), Times.Once);
    }

    [Test]
    public void UpdateSkippedFiles_DoesNotCallClearFiles_WhenNotRequested()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        var skipFilesMock = new Mock<ISkipFiles>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>();
        
        app.UpdateSkippedFiles(skipFilesMock.Object, dictionary);
        
        skipFilesMock.Verify(files => files.ClearFiles(), Times.Never);
    }

    [Test]
    public void UpdateSkippedDirectories_CallsClearDirectories_WhenRequested()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        var skipDirectoriesMock = new Mock<ISkipDirectories>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?> { {"--override-skipdirs", null} };
        
        app.UpdateSkippedDirectories(skipDirectoriesMock.Object, dictionary);
        
        skipDirectoriesMock.Verify(directories => directories.ClearDirectories(), Times.Once);
    }
    
    [Test]
    public void UpdateSkippedDirectories_DoesNotCallClearDirectories_WhenNotRequested()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        var skipDirectoriesMock = new Mock<ISkipDirectories>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>();
        
        app.UpdateSkippedDirectories(skipDirectoriesMock.Object, dictionary);
        
        skipDirectoriesMock.Verify(directories => directories.ClearDirectories(), Times.Never);
    }
}