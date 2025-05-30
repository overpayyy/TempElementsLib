using System;
using System.IO;

public interface ITempDir : ITempElement
{
    bool IsEmpty { get; }
    DirectoryInfo DirInfo { get; }
}