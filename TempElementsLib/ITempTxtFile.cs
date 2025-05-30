public interface ITempTxtFile : ITempFile
{
    string ReadAllText();
    string ReadLine();
    void Write(string text);
    void WriteLine(string text);
}