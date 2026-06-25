using System.Diagnostics; //*
namespace Demo;
public enum ReportType { Collect, Analyze, Recon, Intel }
public enum ReportStatus { Pending, Approved, Rejected }
class Test
{
    static void Main()
    {
        string FileName = "reports.txt";
        string FullPath = FoundPath(FileName);
        
        string[] UnitNameArrey = new string[100];
        string[] ReportTypeArrey = new string[100];
        int[] PriorityArrey = new int[100];
        double[] ScoreArrey = new double[100];
        string[] StatusArrey = new string[100];

        int validLines = ProcessReports(FullPath, FileName, UnitNameArrey, ReportTypeArrey, PriorityArrey, ScoreArrey, StatusArrey);
        Trace.Listeners.Add(new TextWriterTraceListener("log.txt")); //*
        Trace.AutoFlush = true; //*
        
        
        FindMinScore(ScoreArrey, validLines);



    }
    static string FoundPath(string filename) // מציאת הנתיב המדוייק לקובץ הטקסט
    {
        string baseDir = AppDomain.CurrentDomain.BaseDirectory; // חדש
        DirectoryInfo projectDir = Directory.GetParent(baseDir).Parent.Parent.Parent; // חדש
        string FullPath = Path.Combine(projectDir.FullName, filename); // חדש
        return FullPath;
    }
    static int?[] LoadFile(string FullPath, string FileName) // null בדיקת קובץ קיים והדפסת מספר שורות והחזרת מערך של המספר או 
    {
        if (File.Exists(FullPath)) // חדש
        {
            string[] lines = File.ReadAllLines(FullPath); // חדש
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
    static int ProcessReports(string FullPath, string FileName, string[] UnitNameArrey, string[] ReportTypeArrey, int[] PriorityArrey, double[] ScoreArrey, string[] StatusArrey) // עובר בלולאה על השורות מאמת כל אחת ונותן מספר שורות תקינות
    {
        int Valid = 0;
        int Invalid = 0;
        if (LoadFile(FoundPath(FullPath), FileName) != null)
        {
            string[] AllLines = RowAnalysis(FoundPath(FullPath));
            for (int index = 0; index < AllLines.Length; index++)
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
                UnitNameArrey[Valid] = UnitName;
                ReportTypeArrey[Valid] = ReportType;
                PriorityArrey[Valid] = Priority;
                ScoreArrey[Valid] = Score;
                StatusArrey[Valid] = ReportStatus;
                Valid++;
                Trace.WriteLine($"Unit: {lineSplit[0]}\nType: {lineSplit[1]}\nPriority: {lineSplit[2]}\nScore: {lineSplit[3]}\nStatus: {lineSplit[4]}"); //*
            }

            Console.WriteLine("Processing complete.");
            Console.WriteLine($"Valid records: {Valid}");
            Console.WriteLine($"Invalid records: {Invalid}");
            Console.WriteLine($"Stored {Valid} valid records for analysis");
            return Valid;
        }
        else
        {
            return 0;
        }

    }
    static string[] RowAnalysis(string FullPath) // קריאת הקובץ ושמירת המערך
    {
        string[] lines = new string[100];
        if (File.Exists(FullPath)) // חדש
        {
            lines = File.ReadAllLines(FullPath);
            return lines;
        }
        return lines;
    }
    static string[] RowArrey(string AllLines) // הכנסה של כל שורה לשורה נפרדת לצורך בדיקות
    {
        string line = string.Join(Environment.NewLine, AllLines);
        string[] lineToCheck = line.Split(','); // חדש
        return lineToCheck;
    }
    static bool ValidByEnum<T>(string variable) where T : struct, Enum  // פונקציה הבודקת האם הערך נמצא באינום 
    {
        string variableCapital = char.ToUpper(variable[0]) + variable.Substring(1).ToLower(); // חדש
        return Enum.TryParse<T>(variable, true, out _); // חדש

    }
    static int ValidByPriority(int variable) // פונקציית בדיקה האם רמת העדיפות בין 1 ל 5
    {
        if (variable > 0 & variable <= 5)
            return variable;
        return 0;
    }
    static double ValidByScore(double variable) // פונקציית בדיקה האם רמת האיכות בין 0.0 ל100.0
    {
        if (variable > 0.0 & variable <= 100.0)
            return variable;
        return 0;

    }
    static double CalculateAverage(double[] Score, int validLines)
    {
        double sum = 0;
        for (int i = 0; i < validLines; i++)
        {
            
            sum += Score[i];
        }
        double Average = sum / validLines;
        //Console.WriteLine($"{Average:F2}");
        return Average;
    }
    static double FindMaxScore(double[] Score, int validLines)
    {
        double maxScore = 0;
        for (int i = 0; i < validLines; i++)
        {
            if(Score[i] > maxScore)
            {
                maxScore = Score[i];
            }
        }
        //Console.WriteLine(maxScore);
        return maxScore;
    }
    static double FindMinScore(double[] Score, int validLines)
    {
        double minScore = 0;
        {
            for (int i = 0; i < validLines; i++)
            {
                if(Score[i] < minScore)
                {
                    minScore = Score[i];
                }
            }
        }
        //WriteLine($"{minScore:F1}");
        return minScore;
    }
    static void DisplayBasicStatistics(double[] Score, int validLines)
    {

    }
}