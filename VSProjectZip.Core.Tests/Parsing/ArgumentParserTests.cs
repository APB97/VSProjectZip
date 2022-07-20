using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

[TestFixture]
public class ArgumentParserTests
{
    private const string FakeDirectory = "C:/FakeDirectory";
    private const string FakeOutputName = "Output.zip";
    
    [Test]
    public void WhenOneSimpleArgumentPassed_AdditionalArgumentsContainsGivenKeyAndValue()
    {
        var parser = new ArgumentParser(new[]
        {
            $"{ArgumentCollection.OutputDirectory}={FakeDirectory}"
        });
        
        Assert.That(parser.AdditionalArguments, Contains.Key(ArgumentCollection.OutputDirectory).WithValue(FakeDirectory));
    }

    [Test]
    public void WhenTwoArgumentsPassed_AdditionalArgumentsContainsGivenKeysAndValues()
    {
        var parser = new ArgumentParser(new []
        {
            $"{ArgumentCollection.OutputDirectory}={FakeDirectory}",
            $"{ArgumentCollection.OutputName}={FakeOutputName}"
        });
        
        Assert.That(parser.AdditionalArguments,
            Contains.Key(ArgumentCollection.OutputDirectory)
                .WithValue(FakeDirectory)
                .And.ContainKey(ArgumentCollection.OutputName)
                .WithValue(FakeOutputName));
    }

}