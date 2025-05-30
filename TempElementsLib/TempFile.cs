using System;
using System.IO;
using System.Text;

public class TempFile : ITempFile
{
    public readonly FileStream fileStream;
    public readonly FileInfo fileInfo;

    private bool _isDisposed;
    private string _filePath;

    public TempFile()
    {
        try
        {
            _filePath = Path.GetTempFileName();
            fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.ReadWrite);
            fileInfo = new FileInfo(_filePath);
            _isDisposed = false;
        }
        catch (IOException ex)
        {
            throw new IOException("Failed to create temporary file.", ex);
        }
    }

    public string FilePath => _filePath;

    public bool IsDestroyed => _isDisposed;

    public void AddText(string value)
    {
        if (_isDisposed) throw new ObjectDisposedException(nameof(TempFile));
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fileStream.Write(info, 0, info.Length);
        fileStream.Flush();
    }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            try
            {
                fileStream?.Dispose();
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }
                _isDisposed = true;
            }
            catch (IOException ex)
            {
                throw new IOException("Error disposing temporary file.", ex);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }
    }

    ~TempFile()
    {
        Dispose();
    }
}