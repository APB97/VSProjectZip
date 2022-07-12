using Moq;
using VSProjectZip.Core.Logging;
using VSProjectZip.Core.StandardIO.Output;

namespace VSProjectZip.Core.Tests.Logging;

[TestFixture]
public class ConsoleLoggerTests
{
    [Test]
    public void ConsoleLogger_Info_SetsColorToWhite()
    {
        Mock<IConsoleOutput> consoleOutputMock = new Mock<IConsoleOutput>();
        ConsoleLogger logger = new ConsoleLogger();
        
        logger.Info("Sample text");
        
        consoleOutputMock.VerifySet(output => output.Color = ConsoleColor.White, Times.AtLeastOnce);
    }
}