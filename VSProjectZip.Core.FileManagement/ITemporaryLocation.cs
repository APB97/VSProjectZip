namespace VSProjectZip.Core.FileManagement;

public interface ITemporaryLocation
{
    string TemporaryPath { get; }
    void CreateIfDoesNotExist();
    void DeleteIfExists();
}