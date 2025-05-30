using System;
using System.IO;

public interface ITempDir : IDisposable
{
    string DirPath { get; }
    bool IsDestroyed { get; }
    bool IsEmpty { get; }
    DirectoryInfo DirInfo { get; }
}