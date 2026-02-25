using FakeItEasy;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.StandardIO.Output;

namespace VSProjectZip.Core.Tests.Logging;

public class ConsoleLoggerTests
{
    private const string SampleText = "Sample text";
    private readonly ConsoleLogger _consoleLogger = null!;
    private readonly Fake<IConsoleOutput> _consoleOutputMock = null!;

    public ConsoleLoggerTests()
    {
        _consoleOutputMock = new Fake<IConsoleOutput>();
        _consoleLogger = new ConsoleLogger(_consoleOutputMock.FakedObject);
    }
    
    [Fact]
    public void ConsoleLogger_Info_SetsColorToWhite()
    {
        _consoleLogger.Info(SampleText);

        _consoleOutputMock.CallsToSet(output => output.Color).WhenArgumentsMatch(args => args.FirstOrDefault()?.Equals(ConsoleColor.White) ?? false)
            .MustHaveHappenedOnceOrMore();
    }

    [Fact]
    public void ConsoleLogger_Info_CallsWriteLineWithItsArgument()
    {
        _consoleLogger.Info(SampleText);
        
        _consoleOutputMock.CallsTo(output => output.WriteLine(SampleText)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ConsoleLogger_Info_RestoresPreviousColorAfterwards()
    {
        const ConsoleColor previousColor = ConsoleColor.Blue;
        _consoleOutputMock.FakedObject.Color = previousColor;
        
        _consoleLogger.Info(SampleText);
        
        Assert.Equal(previousColor, _consoleOutputMock.FakedObject.Color);
    }
    
    [Fact]
    public void ConsoleLogger_Warn_SetsColorToYellow()
    {
        _consoleLogger.Warn(SampleText);
        
        _consoleOutputMock.CallsToSet(output => output.Color).WhenArgumentsMatch(args => args.FirstOrDefault()?.Equals(ConsoleColor.Yellow) ?? false).MustHaveHappenedOnceOrMore();
    }

    [Fact]
    public void ConsoleLogger_Warn_CallsWriteLineWithItsArgument()
    {
        _consoleLogger.Warn(SampleText);
        
        _consoleOutputMock.CallsTo(output => output.WriteLine(SampleText)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ConsoleLogger_Warn_RestoresPreviousColorAfterwards()
    {
        const ConsoleColor previousColor = ConsoleColor.Blue;
        _consoleOutputMock.FakedObject.Color = previousColor;
        
        _consoleLogger.Warn(SampleText);
        
        Assert.Equal(previousColor, _consoleOutputMock.FakedObject.Color);
    }
    
    [Fact]
    public void ConsoleLogger_Error_SetsColorToRed()
    {
        _consoleLogger.Error(SampleText);
        
        _consoleOutputMock.CallsToSet(output => output.Color).WhenArgumentsMatch(args => args.FirstOrDefault()?.Equals(ConsoleColor.Red) ?? false).MustHaveHappenedOnceOrMore();
    }

    [Fact]
    public void ConsoleLogger_Error_CallsWriteLineWithItsArgument()
    {
        _consoleLogger.Error(SampleText);
        
        _consoleOutputMock.CallsTo(output => output.WriteLine(SampleText)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void ConsoleLogger_Error_RestoresPreviousColorAfterwards()
    {
        const ConsoleColor previousColor = ConsoleColor.Blue;
        _consoleOutputMock.FakedObject.Color = previousColor;
        
        _consoleLogger.Error(SampleText);
        
        Assert.Equal(previousColor, _consoleOutputMock.FakedObject.Color);
    }
}