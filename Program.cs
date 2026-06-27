using System;
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
          
        
        DisplayBasicStatistics(ScoreArrey, validLines);
        DisplayStatusCounts(StatusArrey, validLines);
        DisplayTypeCounts(ReportTypeArrey, validLines);
        DisplayHighestPriorityApproved(UnitNameArrey, ReportTypeArrey, PriorityArrey, ScoreArrey, StatusArrey, validLines);
        DisplayAverageByPriority(PriorityArrey, ScoreArrey, validLines);
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
        if (LoadFile(FullPath, FileName) != null)
        {
            string[] AllLines = RowAnalysis(FullPath);
            for (int index = 0; index < AllLines.Length; index++)
            {
                string[] lineSplit = AllLines[index].Split(',');
                if(lineSplit.Length != 5)
                {
                    Invalid++;
                    continue;
                }
                string UnitName = lineSplit[0].Trim();
                string ReportType = lineSplit[1].Trim();
                if (ValidByEnum<ReportType>(ReportType))
                {
                    ReportType = lineSplit[1].Trim();
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
                string ReportStatus = lineSplit[4].Trim();
                string ValidReportStatus = "";
                if (ValidByEnum<ReportStatus>(ReportStatus))
                {
                    ValidReportStatus = ReportStatus;
                }
                else
                {
                    Invalid++;
                    continue;
                }
                UnitNameArrey[Valid] = UnitName;
                ReportTypeArrey[Valid] = ReportType;
                PriorityArrey[Valid] = Priority;
                ScoreArrey[Valid] = Score;
                StatusArrey[Valid] = ValidReportStatus;
                Valid++;
                Console.WriteLine($"Unit: {lineSplit[0]}\nType: {lineSplit[1]}\nPriority: {lineSplit[2]}\nScore: {lineSplit[3]}\nStatus: {lineSplit[4]}"); //*
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
        if (variable >= 0.0 & variable <= 100.0)
            return variable;
        return 0;

    }
    static double CalculateAverage(double[] Score, int validLines) // מציאת הממוצע של הציונים
    {
        double sum = 0;
        for (int i = 0; i < validLines; i++)
        {
            
            sum += Score[i];
        }
        if (validLines > 0) // אבטחה אישית שלא מתחלק ב0
        {
            double Average = sum / validLines;
            return Average;
        }
        else
            return 0;
        
    }
    static double FindMaxScore(double[] Score, int validLines) // הציון הגבוה ביותר
    {
        double maxScore = 0;
        for (int i = 0; i < validLines; i++)
        {
            if(Score[i] > maxScore)
            {
                maxScore = Score[i];
            }
        }
        return maxScore;
    }
    static double FindMinScore(double[] Score, int validLines) // הציון הנמוך ביותר
    {
        double minScore = 100.0;
        {
            for (int i = 0; i < validLines; i++)
            {
                if(Score[i] < minScore)
                {
                    minScore = Score[i];
                }
            }
        }
        return minScore;
    }
    static void DisplayBasicStatistics(double[] Score, int validLines) // הדפסת הציון הממוצע, הגבוה והנמוך
    {
        double Average = CalculateAverage(Score, validLines);
        double Max = FindMaxScore(Score, validLines);
        double Min = FindMinScore(Score, validLines);
        Console.WriteLine("=== Report Statistics ===");
        Console.WriteLine($"Total Reports: {validLines}");
        Console.WriteLine($"Average Score: {Average:F2}");
        Console.WriteLine($"Highest Score: {Max:F1}");
        Console.WriteLine($"Lowest Score: {Min:F1}");
    }
    static int CountByStatusOrType(string[] Status, int validLines, string statusOrType) // שומר כמות סטטוס לפי מה שמוכנס
    {
        int Conut = 0;
        for (int i = 0; i < validLines; i++)
        {
            if(Status[i].ToLower() == statusOrType.ToLower())
            {
                Conut++;
            }
        }return Conut;
    }
    static void DisplayStatusCounts(string[] Status, int validLines) // מפעיל את פונקציית ספירה לפי סטטוס לכל ססטוס ומדפיס
    {
        Console.WriteLine("=== Reports by Status ===");
        int Pending = CountByStatusOrType(Status, validLines, "Pending");
        Console.WriteLine($"Pending: {Pending}");
        int Approved = CountByStatusOrType(Status, validLines, "Approved");
        Console.WriteLine($"Approved: {Approved}");
        int Rejected = CountByStatusOrType(Status, validLines, "Rejected");
        Console.WriteLine($"Rejected: {Rejected}");
    }
     static void DisplayTypeCounts(string[] ReportType, int validLines) // מפעיל את פונקציית ספירה לפי דו"חות לכל דו"ח ומדפיס
    {
        Console.WriteLine("=== Reports by Type === ");
        int Collect = CountByStatusOrType(ReportType, validLines, "Collect");
        Console.WriteLine($"Collect: {Collect}");
        int Analyze = CountByStatusOrType(ReportType, validLines, "Analyze");
        Console.WriteLine($"Analyze: {Analyze}");
        int Recon = CountByStatusOrType(ReportType, validLines, "Recon");
        Console.WriteLine($"Recon: {Recon}");
        int Intel = CountByStatusOrType(ReportType, validLines, "Intel");
        Console.WriteLine($"Intel: {Intel}");
    }
    static void DisplayHighestPriorityApproved(string[] UnitNameArrey, string[] ReportTypeArrey, int[] PriorityArrey, double[] ScoreArrey, string[] StatusArrey, int validLines)
    {
        string[] Status = StatusArrey;
        string Approved = "Approved";
        int Priority = 0;
        int index = 0;
        for (int i = 0; i < validLines; i++)
        {
            if (Status[i].ToLower() == Approved.ToLower())
            {
                if (PriorityArrey[i] > Priority)
                {
                    Priority = PriorityArrey[i];
                    index = i;
                }
            }
        }
        Console.WriteLine("=== Highest Priority Approved Report ===");
        Console.WriteLine($"Unit: {UnitNameArrey[index]}");
        Console.WriteLine($"Type: {ReportTypeArrey[index]}");
        Console.WriteLine($"Priority: {PriorityArrey[index]}");
        Console.WriteLine($"Score: {ScoreArrey[index]:F1}");
    }
    static void DisplayAverageByPriority(int[] PriorityArrey, double[] ScoreArrey, int validLines)
    {
        double one = 0;
        double oneCont = 0;
        double two = 0;
        double twoCount = 0;
        double three = 0;
        double threeCount = 0;
        double four = 0;
        double fourCount = 0;
        double five = 0;
        double fiveCount = 0;

        for (int i = 0; i < validLines; i++)
        {
            if (PriorityArrey[i] == 1)
            {
                one += ScoreArrey[i];
                oneCont++;
            }
            else if (PriorityArrey[i] == 2)
            {
                two += ScoreArrey[i];
                twoCount++;
            }
            else if (PriorityArrey[i] == 3)
            {
                three += ScoreArrey[i];
                threeCount++;
            }
            else if (PriorityArrey[i] == 4)
            {
                four += ScoreArrey[i];
                fourCount++;
            }
            else if (PriorityArrey[i]== 5)
            {
                five += ScoreArrey[i];
                fiveCount++;
            }
           
        }
        
        Console.WriteLine("=== Average Score by Priority === ");
        if (one > 0)
        {
            Console.WriteLine($"Priority 1: {one / oneCont:F2}");
        }
        else
        {
            Console.WriteLine("Priority 1: No reports");
        }
        if (two > 0)
        {
            Console.WriteLine($"Priority 2: {two / twoCount:F2}");
        }
        else
        {
            Console.WriteLine("Priority 2: No reports");
        }
        if(three > 0)
        {
            Console.WriteLine($"Priority 3: {three / threeCount:F2}");
        }
        else
        {
            Console.WriteLine("Priority 3: No reports");
        }
        if(four > 0)
        {
            Console.WriteLine($"Priority 4: {four / fourCount:F2}");
        }
        else
        {
            Console.WriteLine("Priority 4: No reports");
        }
        if(five > 0)
        {
            Console.WriteLine($"Priority 5: {five / fiveCount:F2}");
        }
        else
        {
            Console.WriteLine("Priority 5: No reports");
        }
    }
}