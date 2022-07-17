using Moq;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Parsing;

[TestFixture]
public class SkippedItemUpdaterTests
{
    
    [Test]
    public void UpdateSkippedFiles_CallsClearFiles_WhenRequested()
    {
        var skipItemsMock = new Mock<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.Object);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?> { {"--override-skipfiles", null} };

        skippedItems.UpdateSkippedFiles(dictionary);
        
        skipItemsMock.Verify(files => files.ClearFiles(), Times.Once);
    }

    [Test]
    public void UpdateSkippedFiles_DoesNotCallClearFiles_WhenNotRequested()
    {
        var skipItemsMock = new Mock<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.Object);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>();
        
        skippedItems.UpdateSkippedFiles(dictionary);
        
        skipItemsMock.Verify(files => files.ClearFiles(), Times.Never);
    }

    [Test]
    public void UpdateSkippedDirectories_CallsClearDirectories_WhenRequested()
    {
        var skipItemsMock = new Mock<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.Object);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?> { {"--override-skipdirs", null} };
        
        skippedItems.UpdateSkippedDirectories(dictionary);
        
        skipItemsMock.Verify(directories => directories.ClearDirectories(), Times.Once);
    }
    
    [Test]
    public void UpdateSkippedDirectories_DoesNotCallClearDirectories_WhenNotRequested()
    {
        var skipItemsMock = new Mock<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.Object);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>();
        
        skippedItems.UpdateSkippedDirectories(dictionary);
        
        skipItemsMock.Verify(directories => directories.ClearDirectories(), Times.Never);
    }
}