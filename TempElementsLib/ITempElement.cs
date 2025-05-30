using System;

public interface ITempElement : IDisposable
{
    string FilePath { get; }
    bool IsDestroyed { get; }
    void PrepareForMove();
}