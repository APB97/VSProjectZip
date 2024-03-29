﻿using VSProjectZip.Core.Utilities;

namespace VSProjectZip.Core.Parsing;

public class SkippedItemsUpdater : ISkippedItemsUpdater
{
    private readonly ISkipItems _skipItems;

    public SkippedItemsUpdater(ISkipItems skipItems)
    {
        _skipItems = skipItems;
    }

    public void UpdateSkippedFiles(IReadOnlyDictionary<string, string?> argumentValues)
    {
        ClearSkippedFilesIfOverrideRequested(argumentValues);
        var skipTheseFiles = DetermineSkippedFiles(argumentValues);
        _skipItems.AddFiles(skipTheseFiles);
    }

    private void ClearSkippedFilesIfOverrideRequested(IReadOnlyDictionary<string, string?> argumentValues)
    {
        bool overrideSkippedFiles = argumentValues.TryGetValue(ArgumentCollection.OverrideSkippedFiles, out _);
        if (!overrideSkippedFiles) return;
        
        _skipItems.ClearFiles();
    }

    private IEnumerable<string> DetermineSkippedFiles(IReadOnlyDictionary<string, string?> argumentValues)
    {
        var skipTheseFiles =
            argumentValues.TryGetValue(ArgumentCollection.SkipFiles, out var skipFiles) && skipFiles is not null
                ? skipFiles.ParseListArgument()
                : Enumerable.Empty<string>();
        
        return skipTheseFiles;
    }

    public void UpdateSkippedDirectories(IReadOnlyDictionary<string, string?> argumentValues)
    {
        ClearSkippedDDirectoriesIfOverrideRequested(argumentValues);
        var skipTheseDirectories = DetermineSkippedDirectories(argumentValues);
        _skipItems.AddDirectories(skipTheseDirectories);
    }

    private void ClearSkippedDDirectoriesIfOverrideRequested(IReadOnlyDictionary<string, string?> argumentValues)
    {
        bool overrideSkippedDirectories = argumentValues.TryGetValue(ArgumentCollection.OverrideSkippedDirectories, out _);
        if (!overrideSkippedDirectories) return;
        
        _skipItems.ClearDirectories();
    }

    private IEnumerable<string> DetermineSkippedDirectories(IReadOnlyDictionary<string, string?> argumentValues)
    {
        return argumentValues.TryGetValue(ArgumentCollection.SkipDirectories, out var skipDirs) && skipDirs is not null
            ? skipDirs.ParseListArgument()
            : Enumerable.Empty<string>();
    }
}