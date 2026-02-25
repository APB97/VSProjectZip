using VSProjectZip.Core.Parsing;

namespace VSProjectZip.Core.Tests.Parsing;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void ParseListArgument_ReturnsEmptySet_WhenNoItemsAreGiven(string emptyArgumentValue)
    {
        var hashSet = emptyArgumentValue.ParseListArgument();
        
        Assert.Empty(hashSet);
    }

    [Fact]
    public void ParseListArgument_ReturnsSetWithGivenItem_WhenOneItemIsGiven()
    {
        string sampleValue = "sampleValue";
        
        var hashSet = sampleValue.ParseListArgument();
        
        Assert.Contains(sampleValue, hashSet);
    }

    [Fact]
    public void ParseListArgument_ReturnsSetWithGivenItems_WhenTwoItemsAreGivenWithWhitespaces()
    {
        string sampleValue = "sampleValue";
        string anotherValue = "anotherValue";
        string listArgumentValue = $"{sampleValue} | {anotherValue}";

        var hashSet = listArgumentValue.ParseListArgument();

        Assert.Contains(sampleValue, hashSet);
        Assert.Contains(anotherValue, hashSet);
    }
    
    [Fact]
    public void ParseListArgument_ReturnsSetWithGivenItems_WhenTwoItemsAreGivenWithNoWhitespaces()
    {
        string sampleValue = "sampleValue";
        string anotherValue = "anotherValue";
        string listArgumentValue = $"{sampleValue}|{anotherValue}";

        var hashSet = listArgumentValue.ParseListArgument();

        Assert.Contains(sampleValue, hashSet);
        Assert.Contains(anotherValue, hashSet);
    }
}
