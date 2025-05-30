using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class TempElementsList : ITempElements
{
    private List<ITempElement> _elements;

    public TempElementsList()
    {
        _elements = new List<ITempElement>();
    }

    public IReadOnlyCollection<ITempElement> Elements => _elements.AsReadOnly();

    public bool IsEmpty => _elements.Count == 0;

    public void AddElement<T>() where T : ITempElement, new()
    {
        if (IsDisposed) throw new ObjectDisposedException(nameof(TempElementsList));
        T element = new T();
        _elements.Add(element);
    }

    public void MoveElementTo<T>(T element, string newPath) where T : ITempElement
    {
        if (IsDisposed) throw new ObjectDisposedException(nameof(TempElementsList));
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (string.IsNullOrEmpty(newPath)) throw new ArgumentException("New path cannot be null or empty.", nameof(newPath));

        string sourcePath = element.FilePath;
        string directory = Path.GetDirectoryName(newPath) ?? string.Empty;
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        element.PrepareForMove();

        if (element is TempFile)
        {
            File.Move(sourcePath, newPath);
            element.Dispose();
            _elements.Remove(element);
            T newElement = (T)Activator.CreateInstance(typeof(T), new object[] { newPath });
            _elements.Add(newElement);
        }
        else if (element is TempDir)
        {
            Directory.Move(sourcePath, newPath);
            element.Dispose();
            _elements.Remove(element);
            T newElement = (T)Activator.CreateInstance(typeof(T), new object[] { newPath });
            _elements.Add(newElement);
        }
    }

    public void DeleteElement<T>(T element) where T : ITempElement
    {
        if (IsDisposed) throw new ObjectDisposedException(nameof(TempElementsList));
        if (element == null) throw new ArgumentNullException(nameof(element));
        element.Dispose();
        _elements.Remove(element);
    }

    public void RemoveDestroyed()
    {
        if (IsDisposed) throw new ObjectDisposedException(nameof(TempElementsList));
        _elements = _elements.Where(e => !e.IsDestroyed).ToList();
    }

    private bool _isDisposed;

    public bool IsDisposed => _isDisposed;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            foreach (var element in _elements)
            {
                element?.Dispose();
            }
            _elements.Clear();
            _isDisposed = true;
        }
    }

    ~TempElementsList()
    {
        Dispose();
    }
}