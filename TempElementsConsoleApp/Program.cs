using System;

class Program
{
    static void Main(string[] args)
    {
        using (var tempFile = new TempFile())
        {
            Console.WriteLine($"Created temporary file at: {tempFile.FilePath}");
            tempFile.AddText("Hello, this is a test!");
            Console.WriteLine("Text added to file.");
        }
        Console.WriteLine("After using block - file should be deleted.");

        var tempFile2 = new TempFile();
        Console.WriteLine($"Created temporary file at: {tempFile2.FilePath}");
        tempFile2.Dispose();
        try
        {
            tempFile2.AddText("This should fail.");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine("Caught ObjectDisposedException: " + ex.Message);
        }

        var tempFile3 = new TempFile();
        try
        {
            tempFile3.AddText("Test without using.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
        }
        finally
        {
            tempFile3.Dispose();
            Console.WriteLine("Manually disposed temporary file.");
        }
    }
}