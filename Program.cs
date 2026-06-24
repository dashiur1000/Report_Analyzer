using System;
using System.IO;
namespace Demo;
class Test
{
    static void Main()
    {
        
        string FileName = "reports.txt";
        LoadFile(FoundPath(FileName), FileName);

    }
    static string FoundPath(string filename) // מציאת הנתיב המדוייק לקובץ הטקסט
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        DirectoryInfo projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent;
        string FullPath = Path.Combine(projectDir.FullName, filename);
        return FullPath;
    }
    static int?[] LoadFile(string FullPath, string FileName) // null בדיקת קובץ קיים והדפסת מספר שורות והחזרת מערך של המספר או 
    { 
        if (File.Exists(FullPath))
        {
            string[] lines = File.ReadAllLines(FullPath);
            int lineCount = lines.Length;
            if (lineCount > 0)
            {
                int?[] lineCountArrey = new int?[lineCount];
                Console.WriteLine($"File loaded: {lineCount} lines found.");
                return lineCountArrey;
            }
            else
            {
                int?[] emptyFile = null;
                Console.WriteLine("Error: File is empty.");
                return emptyFile;
            }       
        }
        else
        {
            int?[] notFound = null;
            Console.WriteLine($"Error: File '{FileName}' not found.");
            return notFound;
        }
    }
    static int ProcessReports(string[] lines, Array[] records, string FullPath, string FileName)
    {
        if(LoadFile(FullPath, FileName) != null)
        {

        }
        else
        {

        }
    }
    static void RowAnalysis(string[] lines)
    {

    }
}