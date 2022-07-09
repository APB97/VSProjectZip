using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

[TestFixture]
public class ArgumentParserTests
{
    private const string OutDir = "--outdir";
    private const string FakeDirectory = "C:/FakeDirectory";
    private const string OutName = "--outname";
    private const string FakeOutputName = "Output.zip";
    
    [Test]
    public void WhenOneSimpleArgumentPassed_AdditionalArgumentsContainsGivenKeyAndValue()
    {
        var parser = new ArgumentParser(new[]
        {
            $"{OutDir}={FakeDirectory}"
        });
        
        Assert.That(parser.AdditionalArguments, Contains.Key(OutDir).WithValue(FakeDirectory));
    }

    [Test]
    public void WhenTwoArgumentsPassed_AdditionalArgumentsContainsGivenKeysAndValues()
    {
        var parser = new ArgumentParser(new []
        {
            $"{OutDir}={FakeDirectory}",
            $"{OutName}={FakeOutputName}"
        });
        
        Assert.That(parser.AdditionalArguments,
            Contains.Key(OutDir)
                .WithValue(FakeDirectory)
                .And.ContainKey(OutName)
                .WithValue(FakeOutputName));
    }

}