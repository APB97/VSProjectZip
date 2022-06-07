namespace VSProjectZip.Core.Utilities
{
    public interface IDirectoryCopier
    {
        void CopyDirectory(string source, string destination);
    }
}