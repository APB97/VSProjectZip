namespace VSProjectZip.Core.Logging;

public interface ILogger
{
    void Info(string value);
    void Warn(string value);
    void Error(string value);
}