using Moq;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.StandardIO.Output;

namespace VSProjectZip.Core.Tests.Logging;

[TestFixture]
public class ConsoleLoggerTests
{
    private const string SampleText = "Sample text";
    private ConsoleLogger _consoleLogger = null!;
    private Mock<IConsoleOutput> _consoleOutputMock = null!;

    [SetUp]
    public void Setup()
    {
        _consoleOutputMock = new Mock<IConsoleOutput>();
        _consoleLogger = new ConsoleLogger(_consoleOutputMock.Object);
    }
    
    [Test]
    public void ConsoleLogger_Info_SetsColorToWhite()
    {
        _consoleLogger.Info(SampleText);
        
        
        _consoleOutputMock.VerifySet(output => output.Color = ConsoleColor.White, Times.AtLeastOnce);
    }

    [Test]
    public void ConsoleLogger_Info_CallsWriteLineWithItsArgument()
    {
        _consoleLogger.Info(SampleText);
        
        
        _consoleOutputMock.Verify(output => output.WriteLine(SampleText));
    }

    [Test]
    public void ConsoleLogger_Info_RestoresPreviousColorAfterwards()
    {
        const ConsoleColor previousColor = ConsoleColor.Blue;
        _consoleOutputMock.SetupProperty(output => output.Color, previousColor);
        
        
        _consoleLogger.Info(SampleText);
        
        
        Assert.That(_consoleOutputMock.Object.Color, Is.EqualTo(previousColor));
    }
}