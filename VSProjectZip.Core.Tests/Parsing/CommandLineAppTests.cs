using Moq;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.Parsing;

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
}