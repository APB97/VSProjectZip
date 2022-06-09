namespace VSProjectZip.Core.Parsing
{
    public static class StringExtensions
    {
        public static HashSet<string> ParseListArgument(this string argumentValue)
        {
            var parsedValues = argumentValue.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var parsedSet = new HashSet<string>(parsedValues);
            return parsedSet;
        }
    }
}
