using Moq;
using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

[TestFixture]
public class CommandLineAppTests
{
    [Test]
    public void DetermineOutputPath_ReturnsProperPath_WhenArgumentsSpecifyDirectoryAndName()
    {
        const string TestOutputDir = "C:/testOut";
        const string Test = "test";
        const string DirectoryToZip = "C:/test";
        var argumentsMock = new Mock<IArgumentHolder>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>
        {
            { ArgumentCollection.OutputName, Test },
            { ArgumentCollection.OutputDirectory, TestOutputDir}
        };
        argumentsMock.SetupGet(holder => holder.AdditionalArguments).Returns(dictionary);
        IArgumentHolder arguments = argumentsMock.Object;
        var app = new CommandLineApp(new DirectoryInfo(DirectoryToZip), arguments);

        var actual = app.DetermineOutputPath();
        
        Assert.That(actual, Is.EqualTo($"{TestOutputDir}{Path.DirectorySeparatorChar}{Test}.zip"));
    }

    [Test]
    public void DetermineOutputPath_ThrowsException_WhenZippingRootDirectory()
    {
        const string Test = "test";
        const string DirectoryToZip = "C:/";
        var argumentsMock = new Mock<IArgumentHolder>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>
        {
            { ArgumentCollection.OutputName, Test }
        };
        argumentsMock.SetupGet(holder => holder.AdditionalArguments).Returns(dictionary);
        IArgumentHolder arguments = argumentsMock.Object;
        var app = new CommandLineApp(new DirectoryInfo(DirectoryToZip), arguments);

        Assert.Catch<Exception>(() => app.DetermineOutputPath());
    }
}