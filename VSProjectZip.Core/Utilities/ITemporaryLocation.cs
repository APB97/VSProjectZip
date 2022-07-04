namespace VSProjectZip.Core.Utilities;

public interface ITemporaryLocation
{
    string TemporaryPath { get; }
    void CreateIfDoesNotExist();
    void DeleteIfExists();
}