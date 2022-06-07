using VSProjectZip.Core.Utilities;
using VSProjectZip.Core.Zipping;

IDirectoryCopier copier = new SkipNamesCopyUtility();
IDirectoryZip zip = new ProjectZip(copier);

var argument = args.FirstOrDefault();
if (argument is not null)
{
    var parentDirectory = new DirectoryInfo(argument).Parent;
    if (parentDirectory is null)
    {
        return;
    }

    zip.ZipDirectory(argument, parentDirectory.FullName);
}