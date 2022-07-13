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

    [Test]
    public void DetermineOutputPath_ReturnsProperPath_WhenArgumentsSpecifyDirectoryAndName()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        const string TestOutputDir = "C:/testOut";
        const string Test = "test";
        const string DirectoryToZip = "C:/test";
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>
        {
            { "--outname", Test },
            { "--outdir", TestOutputDir}
        };

        var actual = app.DetermineOutputPath(dictionary, new DirectoryInfo(DirectoryToZip));
        
        Assert.That(actual, Is.EqualTo($"{TestOutputDir}{Path.DirectorySeparatorChar}{Test}.zip"));
    }

    [Test]
    public void DetermineOutputPath_ThrowsException_WhenZippingRootDirectory()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        const string Test = "test";
        const string DirectoryToZip = "C:/";
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>
        {
            { "--outname", Test }
        };

        Assert.Catch<Exception>(() => app.DetermineOutputPath(dictionary, new DirectoryInfo(DirectoryToZip)));
    }

    [Test]
    public void ReadAdditionalArguments_IncludesSecondArgument()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        const string arg1 = "arg1";
        const string arg2 = "arg2";
        string[] args = { arg1, arg2 };
        
        var dictionary = app.ReadAdditionalArguments(args);
        
        Assert.That(dictionary, Contains.Key(arg2));
    }
    
    [Test]
    public void ReadAdditionalArguments_DoesNotIncludeFirstArgument()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        const string arg1 = "arg1";
        const string arg2 = "arg2";
        string[] args = { arg1, arg2 };
        
        var dictionary = app.ReadAdditionalArguments(args);
        
        Assert.That(dictionary, Does.Not.ContainKey(arg1));
    }

    [Test]
    public void ReadAdditionalArguments_IncludesAllArgumentsExceptFirst()
    {
        var loggerMock = new Mock<ILogger>();
        var app = new CommandLineApp(loggerMock.Object);
        string[] args = Enumerable.Range(1, 10).Select(n => $"arg{n}").ToArray();

        var dictionary = app.ReadAdditionalArguments(args);
        
        Assert.That(dictionary, Does.Not.ContainKey("arg1"));
        Assert.That(dictionary.Count, Is.EqualTo(9));
    }
}