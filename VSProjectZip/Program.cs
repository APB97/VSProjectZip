// See https://aka.ms/new-console-template for more information
using VSProjectZip.Core;

IDirectoryZip zip = new ProjectZip();
var argument = args.FirstOrDefault();
if (argument is not null)
{
    zip.ZipDirectory(argument);
}