namespace VSProjectZip.Core.StandardIO.Output;

public class ConsoleOutput : IConsoleOutput
{
    public static IConsoleOutput Instance { get; } = new ConsoleOutput();
    
    public ConsoleColor Color { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
 
    private ConsoleOutput() { }
    
    public void WriteLine(string value)
    {
        Console.WriteLine(value);
    }
}