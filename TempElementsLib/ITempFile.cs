using System;

public interface ITempFile : IDisposable
{
    string FilePath { get; }
    bool IsDestroyed { get; }
    void Dispose();
    void AddText(string value);
}