using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.StandardIO.Output;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

ILogger logger = new ConsoleLogger(ConsoleOutput.Instance);
DirectoryInfo directoryToZip = new DirectoryInfo(args.First());
IArgumentHolder arguments = new ArgumentParser(args.Skip(1));
var commandLine = new CommandLineApp(directoryToZip, arguments);

string outputPath;
try
{
    outputPath = commandLine.DetermineOutputPath();
}
catch (Exception e)
{
    logger.Error(e.Message);
    throw;
}

IDirectory directoryImplementation = new DirectoryImplementation();
IPath pathImplementation = new PathImplementation();
IFile fileImplementation = new FileImplementation();
SkipNamesCopyUtility copier = new(directoryImplementation, fileImplementation, pathImplementation);
SkippedItemsUpdater skippedItems = new SkippedItemsUpdater(copier);

skippedItems.UpdateSkippedFiles(arguments.AdditionalArguments);
skippedItems.UpdateSkippedDirectories(arguments.AdditionalArguments);

var rootPathName = directoryToZip.Name;

string temporaryPath =
    pathImplementation.Combine(AppContext.BaseDirectory, TemporaryLocation.TempLocationName, rootPathName);
ITemporaryLocation temp = new TemporaryLocation(directoryImplementation, temporaryPath);
IDirectoryZip zip = new ProjectZip(copier, temp, fileImplementation, new ZipFileImplementation());
zip.ZipDirectory(directoryToZip.FullName, outputPath);