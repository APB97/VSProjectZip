using VSProjectZip.Core.FileManagement;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

var directoryToZip = new DirectoryInfo(args.Last());
var arguments = new ArgumentParser(args.SkipLast(1));
var commandLine = new CommandLineApp(directoryToZip, arguments);
var outputPath = commandLine.DetermineOutputPath();

var directoryImplementation = new DirectoryImplementation();
var pathImplementation = new PathImplementation();
var fileImplementation = new FileImplementation();
var fileSystem = new FileSystem(directoryImplementation, fileImplementation, pathImplementation);
var copier = new SkipNamesUtility(fileSystem);
var zipFileImplementation = new ZipFileImplementation();

var skippedItems = new SkippedItemsUpdater(copier);
skippedItems.UpdateSkippedFiles(arguments.AdditionalArguments);
skippedItems.UpdateSkippedDirectories(arguments.AdditionalArguments);

var zip = new ProjectZip(copier, zipFileImplementation, fileImplementation, directoryImplementation,
    arguments.AdditionalArguments.ContainsKey(ArgumentCollection.Force));

await zip.ZipDirectoryAsync(directoryToZip.FullName, outputPath);