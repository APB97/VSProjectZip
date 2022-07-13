using VSProjectZip;
using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.StandardIO.Output;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

var commandLine = new CommandLineApp(new ConsoleLogger(ConsoleOutput.Instance));
var directoryToZip = commandLine.ReadDirectoryToZipFromFirstArgument(args);
var argumentValues = commandLine.ReadAdditionalArguments(args);
var outputPath = commandLine.DetermineOutputPath(argumentValues, directoryToZip);

IDirectory directoryImplementation = new DirectoryImplementation();
IPath pathImplementation = new PathImplementation();
IFile fileImplementation = new FileImplementation();
SkipNamesCopyUtility copier = new(directoryImplementation, fileImplementation, pathImplementation);

commandLine.UpdateSkippedFiles(copier, argumentValues);
commandLine.UpdateSkippedDirectories(copier, argumentValues);

if (directoryToZip is null) return;
var rootPathName = directoryToZip.Name;

string temporaryPath =
    pathImplementation.Combine(AppContext.BaseDirectory, TemporaryLocation.TempLocationName, rootPathName);
ITemporaryLocation temp = new TemporaryLocation(directoryImplementation, temporaryPath);
IDirectoryZip zip = new ProjectZip(copier, temp, fileImplementation, new ZipFileImplementation());
zip.ZipDirectory(directoryToZip.FullName, outputPath);