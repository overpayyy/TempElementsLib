using System;
using System.IO;
using System.Text;

public class TempTxtFile : TempFile, ITempTxtFile
{
    private StreamWriter _streamWriter;
    private StreamReader _streamReader;

    public TempTxtFile() : base()
    {
        InitializeStreams();
    }

    public TempTxtFile(string path) : base(path)
    {
        InitializeStreams();
    }

    private void InitializeStreams()
    {
        _streamWriter = new StreamWriter(fileStream, new UTF8Encoding(true), 1024, true);
        _streamReader = new StreamReader(fileStream, new UTF8Encoding(true), true, 1024, true);
    }

    private void EnsureStreamsAreOpen()
    {
        if (_streamWriter == null || _streamReader == null)
        {
            InitializeStreams();
        }
    }

    private void CloseStreams()
    {
        _streamWriter?.Flush();
        _streamReader?.Close();
        _streamWriter?.Close();
        _streamWriter = null;
        _streamReader = null;
    }

    public string ReadAllText()
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        EnsureStreamsAreOpen();
        fileStream.Position = 0;
        string result = _streamReader.ReadToEnd();
        CloseStreams();
        return result;
    }

    public string ReadLine()
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        EnsureStreamsAreOpen();
        fileStream.Position = 0;
        string result = _streamReader.ReadLine();
        CloseStreams();
        return result;
    }

    public void Write(string text)
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        EnsureStreamsAreOpen();
        _streamWriter.Write(text);
        _streamWriter.Flush();
        CloseStreams();
    }

    public void WriteLine(string text)
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        EnsureStreamsAreOpen();
        _streamWriter.WriteLine(text);
        _streamWriter.Flush();
        CloseStreams();
    }

    public void PrepareForMove()
    {
        CloseStreams();
        base.PrepareForMove();
    }

    public new void Dispose()
    {
        if (!IsDestroyed)
        {
            CloseStreams();
            base.Dispose();
        }
    }
}