using FakeItEasy;
using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

public class CommandLineAppTests
{
    [Fact]
    public void DetermineOutputPath_ReturnsProperPath_WhenArgumentsSpecifyDirectoryAndName()
    {
        const string TestOutputDir = "C:/testOut";
        const string Test = "test";
        const string DirectoryToZip = "C:/test";
        var argumentsMock = new Fake<IArgumentHolder>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>
        {
            { ArgumentCollection.OutputName, Test },
            { ArgumentCollection.OutputDirectory, TestOutputDir}
        };
        argumentsMock.CallsTo(holder => holder.AdditionalArguments).Returns(dictionary);
        IArgumentHolder arguments = argumentsMock.FakedObject;
        var app = new CommandLineApp(new DirectoryInfo(DirectoryToZip), arguments);

        var actual = app.DetermineOutputPath();
        
        Assert.Equal($"{TestOutputDir}{Path.DirectorySeparatorChar}{Test}.zip", actual);
    }

    [Fact]
    public void DetermineOutputPath_ThrowsException_WhenZippingRootDirectory()
    {
        const string Test = "test";
        const string DirectoryToZip = "C:/";
        var argumentsMock = new Fake<IArgumentHolder>();
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>
        {
            { ArgumentCollection.OutputName, Test }
        };
        argumentsMock.CallsTo(holder => holder.AdditionalArguments).Returns(dictionary);
        IArgumentHolder arguments = argumentsMock.FakedObject;
        var app = new CommandLineApp(new DirectoryInfo(DirectoryToZip), arguments);

        Assert.ThrowsAny<Exception>(app.DetermineOutputPath);
    }
}
