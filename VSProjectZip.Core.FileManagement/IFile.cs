﻿namespace VSProjectZip.Core.FileManagement;

public interface IFile
{
    void Copy(string file, string destination, bool overwrite);
}