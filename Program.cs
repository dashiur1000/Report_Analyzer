using System;
using System.IO;
namespace Demo;
public enum ReportType { Collect, Analyze, Recon, Intel }
public enum ReportStatus { Pending, Approved, Rejected }
class Test
{
    static void Main()
    {
        
        string FileName = "reports.txt";
        //LoadFile(FoundPath(FileName), FileName);
        ProcessReports(FileName, FileName);

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
    static int ProcessReports(string FullPath, string FileName) // עובר בלולאה על השורות מאמת כל אחת ונותן מספר שורות תקינות
    {
        int Valid = 0;
        int Invalid = 0;
        if (LoadFile(FoundPath(FullPath), FileName) != null)
        {
            string[] UnitNameArrey = new string[100];
            string[] ReportTypeArrey = new string[100];
            int[] PriorityArrey = new int[100];
            double[] ScoreArrey = new double[100];
            string[] StatusArrey = new string[100];

            string[] AllLines = RowAnalysis(FoundPath(FullPath));
            for (int index =  0; index < AllLines.Length; index ++)
            {
                string[] lineSplit = RowArrey(AllLines[index]);
                string UnitName = lineSplit[0];
                string ReportType = lineSplit[1];
                if (ValidByEnum<ReportType>(lineSplit[1]))
                {
                    ReportType = lineSplit[1];
                }
                else
                {
                    Invalid++;
                    continue;
                }
                    
                int Priority = 0;
                if (int.TryParse(lineSplit[2], out int PriorityToValid))
                {
                    if (ValidByPriority(PriorityToValid) == PriorityToValid)
                    {
                        Priority = PriorityToValid;
                    }
                    else
                    {
                        Invalid++;
                        continue;
                    }
                }
                else
                {
                    Invalid++;
                    continue;
                }
                double Score = 0.0;
                if (double.TryParse(lineSplit[3], out double ScoreToValid))
                {
                    if (ValidByScore(ScoreToValid) == ScoreToValid)
                    {
                        Score = ScoreToValid;
                    }
                    else
                    {
                        Invalid++;
                        continue;
                    }
                }
                else
                {
                    Invalid++;
                    continue;
                }
                string StatusName = lineSplit[4];
                string ReportStatus = lineSplit[4];
                if (ValidByEnum<ReportStatus>(lineSplit[1]))
                {
                    ReportStatus = lineSplit[1];
                }
                UnitNameArrey[index] = UnitName;
                ReportTypeArrey[index] = ReportType;
                PriorityArrey[index] = Priority;
                ScoreArrey[index] = Score;
                StatusArrey[index] = ReportStatus;
                Valid++;
                //Console.WriteLine($"Unit: {lineSplit[0]}\nType: {lineSplit[1]}\nPriority: {lineSplit[2]}\nScore: {lineSplit[3]}\nStatus: {lineSplit[4]}");
            }
            Console.WriteLine("Processing complete.");
            Console.WriteLine($"Valid records: {Valid}");
            Console.WriteLine($"Invalid records: {Invalid}");
            Console.WriteLine($"Stored {Valid} valid records for analysis");
            return ScoreArrey.Length;
        }
        else
        {
            return 0;
        }
        
    }
    static string[] RowAnalysis(string FullPath) // קריאת הקובץ
    {
        string[] lines = File.ReadAllLines(FullPath);
        return lines;
    }
    static string[] RowArrey (string AllLines) // הכנסה של כל שורה לשורה נפרדת לצורך בדיקות
    {
        string line = string.Join(Environment.NewLine, AllLines);
        string[] lineToCheck = line.Split(',');
        return lineToCheck;
    }
    static bool ValidByEnum<T>(string variable) where T : struct, Enum
    {
        string variableCapital = char.ToUpper(variable[0]) + variable.Substring(1).ToLower();
        return Enum.TryParse<T>(variable, true, out _);
        
    } // פונקציה הבודקת האם הערך נמצא באינום  
    static int ValidByPriority(int variable) // פונקציית בדיקה האם רמת העדיפות בין 1 ל 5
    {
        if(variable > 0 & variable <= 5)
            return variable;    
        return 0;
    }
    static double ValidByScore(double variable) // פונקציית בדיקה האם רמת האיכות בין 0.0 ל100.0
    {
        if (variable > 0.0 & variable <= 100.0)
            return variable;
        return 0;
    }
}