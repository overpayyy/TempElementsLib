using System;
using System.IO;

public class TempDir : ITempDir
{
    private string _dirPath;
    private DirectoryInfo _dirInfo;
    private bool _isDisposed;

    public TempDir()
    {
        try
        {
            string tempPath = Path.GetTempPath();
            string randomName = Guid.NewGuid().ToString();
            _dirPath = Path.Combine(tempPath, randomName);
            _dirInfo = Directory.CreateDirectory(_dirPath);
            _isDisposed = false;
        }
        catch (IOException ex)
        {
            throw new IOException("Failed to create temporary directory.", ex);
        }
    }

    public string DirPath => _dirPath;

    public bool IsDestroyed => _isDisposed;

    public bool IsEmpty => _dirInfo.GetFileSystemInfos().Length == 0;

    public DirectoryInfo DirInfo => _dirInfo;

    public void Dispose()
    {
        if (!_isDisposed)
        {
            try
            {
                if (Directory.Exists(_dirPath))
                {
                    Directory.Delete(_dirPath, true);
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