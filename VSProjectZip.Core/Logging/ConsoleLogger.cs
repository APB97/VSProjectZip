using VSProjectZip.Core.StandardIO.Output;

namespace VSProjectZip.Core.Logging;

public class ConsoleLogger : ILogger
{
    private readonly IConsoleOutput _consoleOutput;

    public ConsoleLogger(IConsoleOutput consoleOutput)
    {
        _consoleOutput = consoleOutput;
    }


    public void Info(string value)
    {
        _consoleOutput.Color = ConsoleColor.White;
        _consoleOutput.WriteLine(value);
    }

    public void Warn(string value)
    {
    }

    public void Error(string value)
    {
    }
}