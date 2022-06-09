using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

IDirectoryCopier copier = new SkipNamesCopyUtility();
IDirectoryZip zip = new ProjectZip(copier);

ArgumentParser parser = new(args.Skip(1));
var mainArgument = args.FirstOrDefault();
var argValuePairs = parser.Arguments;
if (mainArgument is not null)
{
    var parentDirectory = new DirectoryInfo(mainArgument).Parent;
    if (parentDirectory is null)
    {
        return;
    }

    string outputDirectory = argValuePairs.TryGetValue("--outdir", out var outDir) && outDir is not null ? outDir : parentDirectory.FullName;
    zip.ZipDirectory(mainArgument, outputDirectory);
}