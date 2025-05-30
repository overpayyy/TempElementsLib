using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        using (var tempElements = new TempElementsList())
        {
            tempElements.AddElement<TempTxtFile>();
            var tempTxtFile = (TempTxtFile)tempElements.Elements.First(e => e is TempTxtFile);
            tempTxtFile.WriteLine("This is a test line.");
            Console.WriteLine($"Created TempTxtFile at: {tempTxtFile.FilePath}");
            Console.WriteLine($"Content: {tempTxtFile.ReadAllText()}");

            string customDirPath = Path.Combine(Path.GetTempPath(), "CustomTempDir");
            var tempDir = new TempDir(customDirPath);
            tempElements.AddElement<TempDir>();
            Console.WriteLine($"Created TempDir at: {tempDir.FilePath}");
            Console.WriteLine($"Is directory empty: {tempDir.IsEmpty}");

            string fileInDirPath = Path.Combine(tempDir.FilePath, "fileInDir.txt");
            File.WriteAllText(fileInDirPath, "File inside directory.");
            Console.WriteLine($"Created file in directory. Is empty: {tempDir.IsEmpty}");

            string newFilePath = Path.Combine(tempDir.FilePath, "movedFile.txt");
            tempElements.MoveElementTo<TempTxtFile>(tempTxtFile, newFilePath);
            tempTxtFile = (TempTxtFile)tempElements.Elements.First(e => e is TempTxtFile);
            Console.WriteLine($"Moved TempTxtFile to: {tempTxtFile.FilePath}");
            Console.WriteLine($"Content after move: {tempTxtFile.ReadAllText()}");

            tempElements.DeleteElement<TempTxtFile>(tempTxtFile);
            Console.WriteLine($"Deleted TempTxtFile. Elements count: {tempElements.Elements.Count()}");

            tempElements.DeleteElement<TempDir>(tempDir);
            Console.WriteLine($"Deleted TempDir. Elements count: {tempElements.Elements.Count()}");

            tempElements.RemoveDestroyed();
            Console.WriteLine($"After RemoveDestroyed, elements count: {tempElements.Elements.Count()}");
        }
        Console.WriteLine("After using block - all elements should be deleted.");
    }
}