using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

DirectoryInfo directoryToZip = new DirectoryInfo(args.First());
IArgumentHolder arguments = new ArgumentParser(args.Skip(1));
var commandLine = new CommandLineApp(directoryToZip, arguments);
var outputPath = commandLine.DetermineOutputPath();

IDirectory directoryImplementation = new DirectoryImplementation();
IPath pathImplementation = new PathImplementation();
IFile fileImplementation = new FileImplementation();
var rootPathName = directoryToZip.Name;
string temporaryPath = TemporaryLocation.GetTemporaryPath(pathImplementation, AppContext.BaseDirectory, rootPathName);
ITemporaryLocation temp = new TemporaryLocation(directoryImplementation, temporaryPath);
IFileSystem fileSystem = new FileSystem(directoryImplementation, fileImplementation, pathImplementation, temp);
SkipNamesCopyUtility copier = new(fileSystem);
IZipFile zipFileImplementation = new ZipFileImplementation();

ISkippedItemsUpdater skippedItems = new SkippedItemsUpdater(copier);
skippedItems.UpdateSkippedFiles(arguments.AdditionalArguments);
skippedItems.UpdateSkippedDirectories(arguments.AdditionalArguments);

IDirectoryZip zip = new ProjectZip(copier, temp, fileImplementation, zipFileImplementation);

zip.ZipDirectory(directoryToZip.FullName, outputPath);