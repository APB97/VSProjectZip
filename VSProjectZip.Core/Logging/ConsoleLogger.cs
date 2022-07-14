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
        OutputWithColor(value, ConsoleColor.White);
    }

    public void Warn(string value)
    {
        OutputWithColor(value, ConsoleColor.Yellow);
    }

    public void Error(string value)
    {
        OutputWithColor(value, ConsoleColor.Red);
    }

    private void OutputWithColor(string value, ConsoleColor desiredColor)
    {
        var currentColor = _consoleOutput.Color;
        _consoleOutput.Color = desiredColor;
        _consoleOutput.WriteLine(value);
        _consoleOutput.Color = currentColor;
    }
}
