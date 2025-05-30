using System;
using System.IO;

public class TempDir : ITempDir
{
    private string _filePath;
    private DirectoryInfo _dirInfo;
    private bool _isDisposed;

    public TempDir()
    {
        try
        {
            string tempPath = Path.GetTempPath();
            string randomName = Guid.NewGuid().ToString();
            _filePath = Path.Combine(tempPath, randomName);
            _dirInfo = Directory.CreateDirectory(_filePath);
            _isDisposed = false;
        }
        catch (IOException ex)
        {
            throw new IOException("Failed to create temporary directory.", ex);
        }
    }

    public TempDir(string path)
    {
        if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        try
        {
            _filePath = path;
            if (Directory.Exists(_filePath))
            {
                Directory.Delete(_filePath, true);
            }
            _dirInfo = Directory.CreateDirectory(_filePath);
            _isDisposed = false;
        }
        catch (IOException ex)
        {
            throw new IOException($"Failed to create directory at {path}.", ex);
        }
    }

    public string FilePath => _filePath;

    public bool IsDestroyed => _isDisposed;

    public bool IsEmpty => _dirInfo.GetFileSystemInfos().Length == 0;

    public DirectoryInfo DirInfo => _dirInfo;

    public void PrepareForMove()
    {
        // Для каталогів не потрібно закривати потоки, але метод присутній для узгодженості
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            try
            {
                if (Directory.Exists(_filePath))
                {
                    Directory.Delete(_filePath, true);
                }
                _isDisposed = true;
            }
            catch (IOException ex)
            {
                throw new IOException("Error disposing temporary directory.", ex);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }

    ~TempDir()
    {
        Dispose();
    }
}