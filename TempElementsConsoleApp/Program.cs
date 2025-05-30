using System;

class Program
{
    static void Main(string[] args)
    {
        using (var tempTxtFile = new TempTxtFile())
        {
            Console.WriteLine($"Created temporary text file at: {tempTxtFile.FilePath}");
            tempTxtFile.Write("Hello, ");
            tempTxtFile.WriteLine("this is a test line!");
            Console.WriteLine("Text written to file.");
            Console.WriteLine("Reading all text: " + tempTxtFile.ReadAllText());
            Console.WriteLine("Reading first line: " + tempTxtFile.ReadLine());
        }
        Console.WriteLine("After using block - file should be deleted.");

        var tempTxtFile2 = new TempTxtFile();
        Console.WriteLine($"Created temporary text file at: {tempTxtFile2.FilePath}");
        tempTxtFile2.Dispose();
        try
        {
            tempTxtFile2.Write("This should fail.");
        }
        catch (ObjectDisposedException ex)
        {
            Console.WriteLine("Caught ObjectDisposedException: " + ex.Message);
        }
    }
}