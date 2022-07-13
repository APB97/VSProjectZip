using VSProjectZip.Core.Logging;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Parsing;

public class CommandLineApp
{
    private readonly ILogger _logger;

    public CommandLineApp(ILogger logger)
    {
        _logger = logger;
    }

    public DirectoryInfo? ReadDirectoryToZipFromFirstArgument(string[] args)
    {
        var mainArgument = args.FirstOrDefault();
        if (mainArgument is not null) return new DirectoryInfo(mainArgument);
    
        _logger.Info(
            "No arguments given. See how to use the app at https://github.com/APB97/VSProjectZip/blob/main/README.md#how-to-use");
        return null;
    }
    
    public IReadOnlyDictionary<string, string?> ReadAdditionalArguments(string[] args)
    {
        ArgumentParser parser = new ArgumentParser(args.Skip(1));
        return parser.AdditionalArguments;
    }
    
    public string DetermineOutputPath(IReadOnlyDictionary<string, string?> argumentValues,
        DirectoryInfo directoryToZip)
    {
        string? outputDirectory = DetermineOutputDirectory(argumentValues, directoryToZip);
        if (outputDirectory is null) throw new ArgumentNullException(nameof(outputDirectory), "Output directory couldn't be determined.");
        
        string outputName = DetermineOutputName(argumentValues, directoryToZip);
        return Path.Combine(outputDirectory, outputName);
    }

    private static string DetermineOutputName(IReadOnlyDictionary<string, string?> argumentValues, DirectoryInfo directoryToZip)
    {
        return argumentValues.TryGetValue("--outname", out var outName) && outName is not null
            ? $"{outName}.zip"
            : $"{directoryToZip.Name}.zip";
    }

    private static string? DetermineOutputDirectory(IReadOnlyDictionary<string, string?> argumentValues, DirectoryInfo directoryToZip)
    {
        return argumentValues.TryGetValue("--outdir", out var outDir) && outDir is not null
            ? outDir
            : directoryToZip.Parent?.FullName;
    }

    public void UpdateSkippedFiles(ISkipFiles copier, IReadOnlyDictionary<string, string?> argumentValues)
    {
        ClearSkippedFilesIfOverrideRequested(copier, argumentValues);
        var skipTheseFiles = DetermineSkippedFiles(argumentValues);
        copier.AddFiles(skipTheseFiles);
    }

    private static void ClearSkippedFilesIfOverrideRequested(ISkipFiles copier,
        IReadOnlyDictionary<string, string?> argumentValues)
    {
        bool overrideSkippedFiles = argumentValues.TryGetValue("--override-skipfiles", out _);
        if (!overrideSkippedFiles) return;
        
        copier.ClearFiles();
    }

    private static IEnumerable<string> DetermineSkippedFiles(IReadOnlyDictionary<string, string?> argumentValues)
    {
        var skipTheseFiles =
            argumentValues.TryGetValue("--skipfiles", out var skipFiles) && skipFiles is not null
                ? skipFiles.ParseListArgument()
                : Enumerable.Empty<string>();
        
        return skipTheseFiles;
    }

    public void UpdateSkippedDirectories(ISkipDirectories copier, IReadOnlyDictionary<string, string?> argumentValues)
    {
        ClearSkippedDDirectoriesIfOverrideRequested(copier, argumentValues);
        var skipTheseDirectories = DetermineSkippedDirectories(argumentValues);
        copier.AddDirectories(skipTheseDirectories);
    }

    private static void ClearSkippedDDirectoriesIfOverrideRequested(ISkipDirectories copier,
        IReadOnlyDictionary<string, string?> argumentValues)
    {
        bool overrideSkippedDirectories = argumentValues.TryGetValue("--override-skipdirs", out _);
        if (!overrideSkippedDirectories) return;
        
        copier.ClearDirectories();
    }

    private static IEnumerable<string> DetermineSkippedDirectories(IReadOnlyDictionary<string, string?> argumentValues)
    {
        return argumentValues.TryGetValue("--skipdirs", out var skipDirs) && skipDirs is not null
            ? skipDirs.ParseListArgument()
            : Enumerable.Empty<string>();
    }
}
