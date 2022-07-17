namespace VSProjectZip.Core.Parsing;

public interface IArgumentHolder
{
    IReadOnlyDictionary<string, string?> AdditionalArguments { get; }
}