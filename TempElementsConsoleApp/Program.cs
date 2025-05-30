using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string customPath = Path.Combine(Path.GetTempPath(), "CustomTempDir");
        using (var tempDir = new TempDir(customPath))
        {
            Console.WriteLine($"Created temporary directory at: {tempDir.FilePath}");
            Console.WriteLine($"Is directory empty: {tempDir.IsEmpty}");

            string testFilePath = Path.Combine(tempDir.FilePath, "test.txt");
            File.WriteAllText(testFilePath, "Test content");
            Console.WriteLine($"Created test file in directory. Is directory empty: {tempDir.IsEmpty}");
        }
        Console.WriteLine("After using block - directory should be deleted.");

        var tempDir2 = new TempDir();
        Console.WriteLine($"Created temporary directory at: {tempDir2.FilePath}");
        tempDir2.Dispose();
        try
        {
            Directory.CreateDirectory(tempDir2.FilePath);
            Console.WriteLine("This should fail.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Caught IOException: " + ex.Message);
        }
    }
}