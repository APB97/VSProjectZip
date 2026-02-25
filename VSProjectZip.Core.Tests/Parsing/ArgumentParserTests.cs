using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

public class ArgumentParserTests
{
    private const string FakeDirectory = "C:/FakeDirectory";
    private const string FakeOutputName = "Output.zip";
    
    [Fact]
    public void WhenOneSimpleArgumentPassed_AdditionalArgumentsContainsGivenKeyAndValue()
    {
        var parser = new ArgumentParser(
        [
            $"{ArgumentCollection.OutputDirectory}={FakeDirectory}"
        ]);

        Assert.Contains(new KeyValuePair<string, string?>(ArgumentCollection.OutputDirectory, FakeDirectory), parser.AdditionalArguments);
    }

    [Fact]
    public void WhenTwoArgumentsPassed_AdditionalArgumentsContainsGivenKeysAndValues()
    {
        var parser = new ArgumentParser(
        [
            $"{ArgumentCollection.OutputDirectory}={FakeDirectory}",
            $"{ArgumentCollection.OutputName}={FakeOutputName}"
        ]);
        
        Assert.Contains(new KeyValuePair<string, string?>(ArgumentCollection.OutputDirectory, FakeDirectory), parser.AdditionalArguments);
        Assert.Contains(new KeyValuePair<string, string?>(ArgumentCollection.OutputName, FakeOutputName), parser.AdditionalArguments);
    }
}
