using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        using (var tempDir = new TempDir())
        {
            Console.WriteLine($"Created temporary directory at: {tempDir.DirPath}");
            Console.WriteLine($"Is directory empty: {tempDir.IsEmpty}");

            string testFilePath = Path.Combine(tempDir.DirPath, "test.txt");
            File.WriteAllText(testFilePath, "Test content");
            Console.WriteLine($"Created test file in directory. Is directory empty: {tempDir.IsEmpty}");
        }
        Console.WriteLine("After using block - directory should be deleted.");

        var tempDir2 = new TempDir();
        Console.WriteLine($"Created temporary directory at: {tempDir2.DirPath}");
        tempDir2.Dispose();
        try
        {
            Directory.CreateDirectory(tempDir2.DirPath);
            Console.WriteLine("This should fail.");
        }
        catch (IOException ex)
        {
            Console.WriteLine("Caught IOException: " + ex.Message);
        }
    }
}