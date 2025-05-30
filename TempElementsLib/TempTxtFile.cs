using System;
using System.IO;
using System.Text;

public class TempTxtFile : TempFile, ITempTxtFile
{
    private StreamWriter _streamWriter;
    private StreamReader _streamReader;

    public TempTxtFile() : base()
    {
        _streamWriter = new StreamWriter(fileStream, new UTF8Encoding(true), 1024, true);
        _streamReader = new StreamReader(fileStream, new UTF8Encoding(true), true, 1024, true);
    }

    public string ReadAllText()
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        fileStream.Position = 0;
        return _streamReader.ReadToEnd();
    }

    public string ReadLine()
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        fileStream.Position = 0;
        return _streamReader.ReadLine();
    }

    public void Write(string text)
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        _streamWriter.Write(text);
        _streamWriter.Flush();
    }

    public void WriteLine(string text)
    {
        if (IsDestroyed) throw new ObjectDisposedException(nameof(TempTxtFile));
        _streamWriter.WriteLine(text);
        _streamWriter.Flush();
    }

    public new void Dispose()
    {
        if (!IsDestroyed)
        {
            _streamWriter?.Dispose();
            _streamReader?.Dispose();
            base.Dispose();
        }
    }
}