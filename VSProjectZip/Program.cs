using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.StandardIO.Output;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

ILogger logger = new ConsoleLogger(ConsoleOutput.Instance);
var commandLine = new CommandLineApp(logger);
var directoryToZip = commandLine.ReadDirectoryToZipFromFirstArgument(args);
if (directoryToZip is null) return;
var argumentValues = commandLine.ReadAdditionalArguments(args);

string outputPath;
try
{
    outputPath = commandLine.DetermineOutputPath(argumentValues, directoryToZip);
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

skippedItems.UpdateSkippedFiles(argumentValues);
skippedItems.UpdateSkippedDirectories(argumentValues);

var rootPathName = directoryToZip.Name;

string temporaryPath =
    pathImplementation.Combine(AppContext.BaseDirectory, TemporaryLocation.TempLocationName, rootPathName);
ITemporaryLocation temp = new TemporaryLocation(directoryImplementation, temporaryPath);
IDirectoryZip zip = new ProjectZip(copier, temp, fileImplementation, new ZipFileImplementation());
zip.ZipDirectory(directoryToZip.FullName, outputPath);