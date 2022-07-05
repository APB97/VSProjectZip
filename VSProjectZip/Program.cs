using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

ArgumentParser parser = new(args.Skip(1));
var mainArgument = args.FirstOrDefault();
var argValuePairs = parser.AdditionalArguments;
if (mainArgument is not null)
{
    var directoryToZip = new DirectoryInfo(mainArgument);
    var parentDirectory = directoryToZip.Parent;
    if (parentDirectory is null)
    {
        return;
    }

    string outputDirectory = argValuePairs.TryGetValue("--outdir", out var outDir) && outDir is not null ? outDir : parentDirectory.FullName;
    string outputName = argValuePairs.TryGetValue("--outname", out var outName) && outName is not null ? outName : $"{directoryToZip.Name}.zip";
    string outputPath = Path.Combine(outputDirectory, outputName);

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
    
    IDirectory directoryImplementation = new DirectoryImplementation();
    IPath pathImplementation = new PathImplementation();
    IFile fileImplementation = new FileImplementation();
    SkipNamesCopyUtility copier = new(directoryImplementation, fileImplementation, pathImplementation);
    var rootPathName = directoryToZip.Name;

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
    string temporaryPath = pathImplementation.Combine(AppContext.BaseDirectory, TemporaryLocation.TempLocationName, rootPathName);
    ITemporaryLocation temp = new TemporaryLocation(directoryImplementation, temporaryPath);
    IDirectoryZip zip = new ProjectZip(copier, temp, fileImplementation, new ZipFileImplementation());
    zip.ZipDirectory(mainArgument, outputPath);
}