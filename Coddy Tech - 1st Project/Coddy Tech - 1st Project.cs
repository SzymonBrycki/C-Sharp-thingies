

using System; // Don't delete this line
using System.Collections.Generic;

public class DataCollector
{
    public static int[][] CreateScoreGrid(int students, int assignments)
    {
        // Write your code here
        int[][] myscoreGrid = new int[students][];

        for (int i = 0; i < students; i++)
        {
            myscoreGrid[i] = new int[assignments];
        }

        return myscoreGrid;
    }
    
    public static bool ValidateScore(int score)
    {
       // Write your code here
       if (score >=0 && score <= 100)
       {
            return true;
       }

       else
       {
            return false;
       }
    }
    
    public static int[][] PopulateWithDefaultValues(int[][] scoreGrid)
    {
        // Write your code here
        for (int a = 0; a < scoreGrid.Length; a++)
        {
            for (int b = 0; b < scoreGrid[a].Length; b++)
            {
                scoreGrid[a][b] = -1;
            }
        }

        return scoreGrid;
    }
}

public class DataEntry
{
    public static int SetStudentScore(int[][] scoreGrid, int studentIndex, int assignmentIndex, int score)
    {
        if (studentIndex < 0 || studentIndex >= scoreGrid.Length || 
        assignmentIndex < 0 || assignmentIndex >= scoreGrid[0].Length)
        {
            return -1;
        }

        if (!DataCollector.ValidateScore(score))
        {
            int error = -2;
            // Console.WriteLine("Result code: " + error); 
            return error;
        }

    scoreGrid[studentIndex][assignmentIndex] = score;
    return 0;

    }

    public static int UpdateAllScores(int[][] scoreGrid, int[] studentIndices, int assignmentIndex, int score)
    {
        int myCount = 0;

        for (int a = 0; a < studentIndices.Length; a++)
        {
            try
            {
                if (DataEntry.SetStudentScore(scoreGrid, studentIndices[a], assignmentIndex, score) == 0)
                {
                    myCount++;
                }
            }

            catch (InvalidScoreException)
            {
                int myError = -2;
                //Console.WriteLine("Result code: " + myError);
                return myError;
            }
        }

        return myCount;
    }

}

public class DataAnalyzer
{
    public static double CalculateStudentAverage(int[][] scoreGrid, int studentIndex)
    {
        try
        {
            double average = 0.0;
            int sum = 0;
            int number = 0;

            for (int i = 0; i < scoreGrid[studentIndex].Length; i++)
            {
                if (scoreGrid[studentIndex][i] < 0)
                {
                    continue;
                }

                sum = sum + scoreGrid[studentIndex][i];
                number++;
            }

            average = (double) sum / (double) number;
            return average;
        }

        catch (IndexOutOfRangeException)
        {
            return -1;
        }
    }

    public static double CalculateAssignmentAverage(int[][] scoreGrid, int assignmentIndex)
    {
        try
        {
            double average = 0.0;
            int sum = 0;
            int number = 0;

            for (int a = 0; a < scoreGrid.Length; a++)
            {

                if (scoreGrid[a][assignmentIndex] < 0)
                {
                    continue;
                }

                sum = sum + scoreGrid[a][assignmentIndex];
                number++;
            }

            if(number == 0)
            {
                return 0;
            }
            
            average = (double) sum / (double) number;
            return average;
        }

        catch (IndexOutOfRangeException)
        {
            return -1;
        }
    }

    public static int[] FindHighestScore(int[][] scoreGrid)
    {
        int studentIndex = 0;
        int assignmentIndex = 0;
        int score = 0;
        for (int a = 0; a < scoreGrid.Length; a++)
        {
            for (int b = 0; b < scoreGrid[a].Length; b++)
            {
                if (scoreGrid[a][b] > score)
                {
                    score = scoreGrid[a][b];
                    studentIndex = a;
                    assignmentIndex = b;
                }
            }
        }

        return new int[] { studentIndex, assignmentIndex, score };
    }
}

public class GradingSystem
{
    public static string ConvertToLetterGrade(double score)
    {
        if (score >= 90) return "A";
        else if (score >= 80) return "B";
        else if (score >= 70) return "C";
        else if (score >= 60) return "D";
        else if (score >= 0) return "F";
        else return "N/A";
    }

    public static string GetStudentGrade(int[][] scoreGrid, int studentIndex)
    { 
        double avg = DataAnalyzer.CalculateStudentAverage(scoreGrid, studentIndex);
        
        try
        {
            if (avg < 0) return "N/A"; 
            
            return ConvertToLetterGrade(avg);
        } 

        catch (IndexOutOfRangeException)
        {
            return "N/A";
        }

        catch (InvalidScoreException)
        {
            return "N/A";
        }
    }

    public static int[] GetClassDistribution(int[][] scoreGrid)
    {
        int A = 0;
        int B = 0;
        int C = 0;
        int D = 0;
        int F = 0;
    
        for (int a = 0; a < scoreGrid.Length; a++)
        {

            string myString = GetStudentGrade(scoreGrid, a);

            switch(myString)
            {
                case "A":
                    A++;
                    break;
                
                case "B":
                    B++;
                    break;

                case "C":
                    C++;
                    break;

                case "D":
                    D++;
                    break;
                
                case "F":
                    F++;
                    break;
                
                default:
                    throw  new InvalidLetterScoreException();
                    break;
            }
        }

        int[] classScores = new int[] { A, B, C, D, F };
        return classScores;
    }
}

public class ReportGenerator
{
    public static string GenerateStudentReport(int[][] scoreGrid, int studentIndex)
    {
        string myString;
        string student;
        string average;
        string grade;
        string scores = "";

        try
        {
        for (int a = 0; a < scoreGrid[studentIndex].Length; a++)
            {
                if (scoreGrid[studentIndex][a] < 0)
                {
                    scores = scores + "N/A, ";
                }

                else
                {
                scores = scores + scoreGrid[studentIndex][a] + ", ";
                }
            }
        }

        catch (IndexOutOfRangeException)
        {
            return "Invalid student index";
        }

        student = Convert.ToString(studentIndex);
        average = Convert.ToString(Math.Round(DataAnalyzer.CalculateStudentAverage(scoreGrid, studentIndex), 1));
        grade = GradingSystem.ConvertToLetterGrade(Math.Round(DataAnalyzer.CalculateStudentAverage(scoreGrid, studentIndex), 1)) + "\n";

        string scores2 = scores.Remove(scores.Length-2);

        return "Student #" + student + " | Average: " + average + " | Grade: " + grade + "Assignment scores: " + scores2;
    }

    public static string GenerateClassSummary(int[][] scoreGrid)
    {
        if (scoreGrid.Length == 0)
        {
            return "Class Summary\nTotal Students: 0\nClass Average: 0.0\nGradeDistribution: A: 0, B: 0, C: 0, D: 0. F: 0";
        }

        else
        {

            string students = Convert.ToString(scoreGrid.Length);

            double sum = 0;
            int counter = 0;
            double average = 0;

            for (int a = 0; a < scoreGrid.Length; a++)
            {
                sum = sum + DataAnalyzer.CalculateStudentAverage(scoreGrid, a);
                counter++;
            }

            average = sum / counter;

            double average2 = Math.Round(average, 1);

            string classAverage = Convert.ToString(average2);

            // int[] gradeDistribution = GradingSystem.GetClassDistribution(scoreGrid);

            int[] gradeDistribution;

            try 
            {
                gradeDistribution = GradingSystem.GetClassDistribution(scoreGrid);
            }
            
            catch (InvalidLetterScoreException)
            {
                return "Error: scoreGrid contains invalid scores that cannot be processed.";
            }

            string gradeDistributionStr = "A: " + gradeDistribution[0] + ", B: " +gradeDistribution[1] + ", C: " + gradeDistribution[2] + ", D: " +gradeDistribution[3] + ", F: " + gradeDistribution[4];
        
            string myString = "Class Summary\n" + "Total Students: " + students + "\nClass Average: " + classAverage + "\nGrade Distribution: " + gradeDistributionStr;
        
            return myString;
        }  
    }

    public static string GenerateAssignmentReport(int[][] scoreGrid, int assignmentIndex)
    {
        if (scoreGrid == null || scoreGrid.Length == 0 || assignmentIndex < 0 || assignmentIndex >= scoreGrid[0].Length)
        {
            return "Invalid assignment index";
        }

        double sum = 0;
        int completed = 0;
        int totalStudents = scoreGrid.Length;

        for (int i = 0; i < totalStudents; i++)
        {
            if (scoreGrid[i][assignmentIndex] >= 0)
            {
                sum += scoreGrid[i][assignmentIndex];
                completed++;
            }
        }

       // 2. Średnia (tylko z oddanych prac)
        double average = (completed > 0) ? (double)sum / completed : 0.0;
        string avgStr = average.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);

        double completedRatio = ((double)completed / totalStudents) * 100;

        string ratioStr;
        if (completedRatio % 1 == 0)
        {
            ratioStr = completedRatio.ToString("0");
        }
        else
        {
            ratioStr = completedRatio.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
        }

        return "Assignment #" + assignmentIndex + " | Average: " + avgStr + " | Completion Rate: " + ratioStr + "%";
    }  
} 

public class ErrorHandler
{
    public static string ValidateInput(int[][] scoreGrid, int studentIndex, int assignmentIndex)
    {
        try
        {
            // int return1;

            //return1 = scoreGrid[studentIndex][0];
        
            if (studentIndex >= scoreGrid.Length)
            {
                throw new IndexOutOfRangeException();
            }

        }

        catch (IndexOutOfRangeException)
        {
            return "Invalid student index";
        }

        try
        {
            // int return2;
            // return2 = scoreGrid[studentIndex][assignmentIndex];

            if (assignmentIndex >= scoreGrid[studentIndex].Length)
            {
                throw new IndexOutOfRangeException();
            }
       
        }

        catch (IndexOutOfRangeException)
        {
            return "Invalid assignment index";
        }

        return "";
    }

    public static int SafeGetScore(int[][] scoreGrid, int studentIndex, int assignmentIndex)
    {
        try
        {
            int result = scoreGrid[studentIndex][assignmentIndex];
            return result;
        }

        catch (IndexOutOfRangeException)
        {
            return -999;
        }
    }

    public static string[] ProcessBatchUpdate(int[][] scoreGrid, int[][] updates)
    {
        var list = new List<String>();

        for (int i = 0; i < updates.Length; i++)
        {
            if (updates[i].Length != 3)
            {
                string myString = $"Error at index {i}; Invalid update format";
                list.Add(myString);
                
                continue;
            }

            else
            {

                int myStudentIndex = updates[i][0];
                int myAssignmentIndex = updates[i][1];
                int myScore = updates[i][2];

                int myDataEntry = DataEntry.SetStudentScore(scoreGrid, myStudentIndex, myAssignmentIndex, myScore);
            
                if (myDataEntry == -1)
                {
                    string myValidate = ValidateInput(scoreGrid, myStudentIndex, myAssignmentIndex);
                    string myValidate2 = $"Error at index {i}: " + myValidate;
                    list.Add(myValidate2);
                }

                if (myDataEntry == -2)
                {
                    string myReturn = $"Error at index {i}: Invalid score value";
                    list.Add(myReturn);
                }
            }

            
        }

        string[] returnArray = list.ToArray();
        return returnArray;
    }
}

public class InvalidScoreException: Exception
{
    public InvalidScoreException()
    {
    }

    public InvalidScoreException(string message) : base(message)
    {
    }
}

public class InvalidLetterScoreException: Exception
{
    public InvalidLetterScoreException()
    {

    }

    public InvalidLetterScoreException(string message) : base(message)
    {

    }
} 