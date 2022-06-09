namespace VSProjectZip.Core.Parsing
{
    public class ArgumentParser
    {
        public IReadOnlyDictionary<string, string?> AdditionalArguments { get; private set; }

        public ArgumentParser(IEnumerable<string> args)
        {
            AdditionalArguments = args.Select(arg => arg.Split('=', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Where(array => array.Length > 0)
                .ToDictionary(nameValueArray => nameValueArray.First(), nameValueArray => nameValueArray.LastOrDefault());
        }       
    }
}
