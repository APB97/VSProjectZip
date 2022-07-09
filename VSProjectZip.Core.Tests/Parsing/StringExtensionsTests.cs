using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

[TestFixture]
public class StringExtensionsTests
{
    [TestCase("")]
    [TestCase(" ")]
    public void ParseListArgument_ReturnsEmptySet_WhenNoItemsAreGiven(string emptyArgumentValue)
    {
        var hashSet = emptyArgumentValue.ParseListArgument();
        
        Assert.That(hashSet, Is.Empty);
    }

    [Test]
    public void ParseListArgument_ReturnsSetWithGivenItem_WhenOneItemIsGiven()
    {
        string sampleValue = "sampleValue";
        
        var hashSet = sampleValue.ParseListArgument();
        
        Assert.That(hashSet, Contains.Item(sampleValue));
    }

    [Test]
    public void ParseListArgument_ReturnsSetWithGivenItems_WhenTwoItemsAreGivenWithWhitespaces()
    {
        string sampleValue = "sampleValue";
        string anotherValue = "anotherValue";
        string listArgumentValue = $"{sampleValue} | {anotherValue}";

        var hashSet = listArgumentValue.ParseListArgument();
        
        Assert.That(hashSet, Contains.Item(sampleValue).And.Contains(anotherValue));
    }
    
    [Test]
    public void ParseListArgument_ReturnsSetWithGivenItems_WhenTwoItemsAreGivenWithNoWhitespaces()
    {
        string sampleValue = "sampleValue";
        string anotherValue = "anotherValue";
        string listArgumentValue = $"{sampleValue}|{anotherValue}";

        var hashSet = listArgumentValue.ParseListArgument();
        
        Assert.That(hashSet, Contains.Item(sampleValue).And.Contains(anotherValue));
    }
    
}