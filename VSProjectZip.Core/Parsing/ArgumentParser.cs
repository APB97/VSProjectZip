namespace VSProjectZip.Core.Parsing
{
    public class ArgumentParser(IEnumerable<string> args) : IArgumentHolder
    {
        private const StringSplitOptions RemoveEmptyEntriesAndTrim = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;

        public IReadOnlyDictionary<string, string?> AdditionalArguments { get; } = 
            args.Select(arg => arg.Split('=', RemoveEmptyEntriesAndTrim))
                .Where(array => array.Length > 0)
                .ToDictionary(nameValueArray => nameValueArray.First(),
                    nameValueArray => nameValueArray.Skip(1).LastOrDefault());
    }
}
