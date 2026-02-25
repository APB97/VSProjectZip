using FakeItEasy;
using VSProjectZip.Core.Parsing;
using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Tests.Parsing;

public class SkippedItemUpdaterTests
{
    [Fact]
    public void UpdateSkippedFiles_CallsClearFiles_WhenRequested()
    {
        var skipItemsMock = new Fake<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.FakedObject);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?> { {ArgumentCollection.OverrideSkippedFiles, null} };

        skippedItems.UpdateSkippedFiles(dictionary);
        
        skipItemsMock.CallsTo(files => files.ClearFiles()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void UpdateSkippedFiles_DoesNotCallClearFiles_WhenNotRequested()
    {
        var skipItemsMock = new Fake<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.FakedObject);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>();
        
        skippedItems.UpdateSkippedFiles(dictionary);
        
        skipItemsMock.CallsTo(files => files.ClearFiles()).MustNotHaveHappened();
    }

    [Fact]
    public void UpdateSkippedDirectories_CallsClearDirectories_WhenRequested()
    {
        var skipItemsMock = new Fake<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.FakedObject);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?> { {ArgumentCollection.OverrideSkippedDirectories, null} };
        
        skippedItems.UpdateSkippedDirectories(dictionary);
        
        skipItemsMock.CallsTo(directories => directories.ClearDirectories()).MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public void UpdateSkippedDirectories_DoesNotCallClearDirectories_WhenNotRequested()
    {
        var skipItemsMock = new Fake<ISkipItems>();
        var skippedItems = new SkippedItemsUpdater(skipItemsMock.FakedObject);
        IReadOnlyDictionary<string,string?> dictionary = new Dictionary<string, string?>();
        
        skippedItems.UpdateSkippedDirectories(dictionary);
        
        skipItemsMock.CallsTo(directories => directories.ClearDirectories()).MustNotHaveHappened();
    }
}
