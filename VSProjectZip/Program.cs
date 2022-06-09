using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

ArgumentParser parser = new(args.Skip(1));
var mainArgument = args.FirstOrDefault();
var argValuePairs = parser.AdditionalArguments;
if (mainArgument is not null)
{
    var parentDirectory = new DirectoryInfo(mainArgument).Parent;
    if (parentDirectory is null)
    {
        return;
    }

    string outputDirectory = argValuePairs.TryGetValue("--outdir", out var outDir) && outDir is not null ? outDir : parentDirectory.FullName;
    bool overrideSkippedDirectories = argValuePairs.TryGetValue("--override-skipdirs", out _);
    bool overrideSkippedFiles = argValuePairs.TryGetValue("--override-skipfiles", out _);
    var skipTheseDirectories =
        argValuePairs.TryGetValue("--skipdirs", out var skipDirs) && skipDirs is not null 
        ? skipDirs.ParseListArgument()
        : Enumerable.Empty<string>();
    var skipTheseFiles =
        argValuePairs.TryGetValue("--skipfiles", out var skipFiles) && skipFiles is not null
        ? skipFiles.ParseListArgument()
        : Enumerable.Empty<string>();
    SkipNamesCopyUtility copier = new();

    if (overrideSkippedDirectories)
    {
        copier.ClearDirectories();
    }

    if (overrideSkippedFiles)
    {
        copier.ClearFiles();
    }

    copier.AddDirectories(skipTheseDirectories);
    copier.AddFiles(skipTheseFiles);
    IDirectoryZip zip = new ProjectZip(copier);
    zip.ZipDirectory(mainArgument, outputDirectory);
}