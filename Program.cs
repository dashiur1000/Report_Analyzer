using System.Diagnostics; //*
namespace Demo;
public enum ReportType { Collect, Analyze, Recon, Intel }
public enum ReportStatus { Pending, Approved, Rejected }
class Test
{
    static void Main()
    {
        Trace.Listeners.Add(new TextWriterTraceListener("log.txt")); //*
        Trace.AutoFlush = true; //*
        string FileName = "reports.txt";
        //LoadFile(FoundPath(FileName), FileName);
        ProcessReports(FoundPath(FileName), FileName);
        CalculateAverage(ValidationOfHealth(FoundPath(FileName), FileName));

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
                UnitNameArrey[index] = UnitName;
                ReportTypeArrey[index] = ReportType;
                PriorityArrey[index] = Priority;
                ScoreArrey[index] = Score;
                StatusArrey[index] = ReportStatus;
                Valid++;
                Trace.WriteLine($"Unit: {lineSplit[0]}\nType: {lineSplit[1]}\nPriority: {lineSplit[2]}\nScore: {lineSplit[3]}\nStatus: {lineSplit[4]}"); //*
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
    static object[] ValidationOfHealth(string FullPath, string FileName) // מייצר מערך של כל המערכים לצורך ביצוע חישובים
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
            for (int index = 0; index < AllLines.Length; index++)
            {
                int WhileLoop = 1;
                while (WhileLoop == 1)
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
                        WhileLoop = 0;
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
                            WhileLoop = 0;
                        }
                    }
                    else
                    {
                        Invalid++;
                        WhileLoop = 0;
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
                            WhileLoop = 0;
                        }
                    }
                    else
                    {
                        Invalid++;
                        WhileLoop = 0;
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
                    WhileLoop = 0;
                }
            }
            
            object[] AllArrays = new object[5];

            AllArrays[0] = UnitNameArrey;
            AllArrays[1] = ReportTypeArrey;
            AllArrays[2] = PriorityArrey;
            AllArrays[3] = ScoreArrey;
            AllArrays[4] = StatusArrey;
      
            return AllArrays;
        }
        else
        {
            object[] AllArrays = new object[1];
            return AllArrays;
        }

    }
    static double CalculateAverage(object[] AllArrays, string FileName)
    {
        double[] Score =(double[])AllArrays[3];
        double sum = 0;
        for (int i = 0; i < ProcessReports(FoundPath(FileName), FileName); i++)
        {
            Console.WriteLine($"{i+1}: {Score[i]}");
        }
        return sum;
    }
}