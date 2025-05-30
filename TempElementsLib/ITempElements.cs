using System;
using System.Collections.Generic;

public interface ITempElements : IDisposable
{
    IReadOnlyCollection<ITempElement> Elements { get; }
    bool IsEmpty { get; }
    void AddElement<T>() where T : ITempElement, new();
    void MoveElementTo<T>(T element, string newPath) where T : ITempElement;
    void DeleteElement<T>(T element) where T : ITempElement;
    void RemoveDestroyed();
}