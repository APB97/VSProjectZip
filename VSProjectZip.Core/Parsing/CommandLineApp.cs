namespace VSProjectZip.Core.Parsing;

public class CommandLineApp
{
    private readonly DirectoryInfo _directoryToZip;
    private readonly IArgumentHolder _arguments;

    public CommandLineApp(DirectoryInfo directoryToZip, IArgumentHolder arguments)
    {
        _directoryToZip = directoryToZip;
        _arguments = arguments;
    }
    
    public string DetermineOutputPath()
    {
        string? outputDirectory = DetermineOutputDirectory();
        if (outputDirectory is null) throw new ArgumentNullException(nameof(outputDirectory), "Output directory couldn't be determined.");
        
        string outputName = DetermineOutputName();
        return Path.Combine(outputDirectory, outputName);
    }

    private string DetermineOutputName()
    {
        var argumentValues = _arguments.AdditionalArguments;
        return argumentValues.TryGetValue(ArgumentCollection.OutputName, out var outName) && outName is not null
            ? $"{outName}.zip"
            : $"{_directoryToZip.Name}.zip";
    }

    private string? DetermineOutputDirectory()
    {
        var argumentValues = _arguments.AdditionalArguments;
        return argumentValues.TryGetValue(ArgumentCollection.OutputDirectory, out var outDir) && outDir is not null
            ? outDir
            : _directoryToZip.Parent?.FullName;
    }
}
