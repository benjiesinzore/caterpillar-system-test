using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

class GECA
{
    

    string map = @"
        $*********$**********$********
        ***$*******B*************#****
        ************************#*****
        ***#**************************
        **$*************************#*
        $$***#************************
        **************$***************
        **********$*********$*****#***
        ********************$*******$*
        *********#****$***************
        **B*********$*****************
        *************$$****B**********
        ****$************************B
        **********************#*******
        ***********************$***B**
        ********$***$*****************
        ************$*****************
        *********$********************
        *********************#********
        *******$**********************
        *#***$****************#*******
        ****#****$****$********B******
        ***#**$********************$**
        ***************#**************
        ***********$******************
        ****B****#******B*************
        ***$***************$*****B**** 
        **********$*********#*$*******
        **************#********B******
        H**********$*********#*B******
        ";




    public void DisplayMap()
    {
        string mapToDisplay = mapRunFirst ? map : newMap;

        foreach (char c in mapToDisplay)
        {
            Console.Write(c);
            Thread.Sleep(1);
        }
    }



    public void ReduceRowsTo11()
    {

        var lines = new string[0];

        if (mapRunFirst)
        {
            lines = map.Split("\n");
        }
        else
        {
            lines = newMap.Split("\n");
        }


        string lineWithH = lines.FirstOrDefault(line => line.Contains('H'));

        int index = lines.ToList().IndexOf(lineWithH);

        int index_ = lines[index].Trim().IndexOf('H');

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < lines.Length; i++)
        {
            var getLine = lines[i];
            int distance = Math.Abs(index - i);

            if (distance <= 5)
            {
                int columnReduction = distance switch
                {
                    0 => 4,
                    1 => 3,
                    2 => 3,
                    3 => 3,
                    4 => 2,
                    5 => 1,
                    _ => 0,
                };
                sb.Append("        " + ReduceColmnTo11(getLine, columnReduction, index_) + "\n");
            }
        }



        Console.Write(sb);
    }

    public string ReduceColmnTo11(string row, int column, int index_)
    {
        StringBuilder sb = new StringBuilder(row.Trim());
        StringBuilder sb__ = new StringBuilder();

        sb__.Append(" ");
        for (int i = 0; i < sb.Length; i++)
        {
            switch (column)
            {
                case 1:
                    sb__.Append(index_ == i ? sb[i].ToString() : " ");
                    break;
                case 2:
                    sb__.Append(Math.Abs(index_ - i) <= 3 ? sb[i].ToString() : " ");
                    break;
                case 3:
                    sb__.Append(Math.Abs(index_ - i) <= 4 ? sb[i].ToString() : " ");
                    break;
                case 4:
                    sb__.Append(Math.Abs(index_ - i) <= 5 ? sb[i].ToString() : " ");
                    break;
            }

        }



        return sb__.ToString();
    }

    private string newMap;
    private string previousState;
    private bool mapRunFirst = true;

    public void UpdateMap(int displacement, string direction, bool expand, bool shrink)
    {
        string currentMap = mapRunFirst ? map : newMap;

        (int row, int column) = LocateCatapilHead(currentMap);

        string mapWithStars = ChangeHeadTailtoStar(currentMap);

        string updatedMap;

        if (expand)
        {
            updatedMap = MoveHeadTailOnMap(mapWithStars, row, column, displacement, direction, expand);
        }
        else
        {
            updatedMap = MoveHeadTailOnMap(mapWithStars, row, column, displacement, direction, expand);
        }

        if (mapRunFirst)
        {
            newMap = updatedMap;
            mapRunFirst = false;
        }
        else
        {
            previousState = newMap;
            newMap = updatedMap;
        }
    }



    public void UndoCommand()
    {
        if (!mapRunFirst)
        {
            newMap = previousState;
        }
    }


    public string ChangeHeadTailtoStar(string originalString)
    {
        var lines = originalString.Split('\n');

        
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Contains('H'))
            {
                
                lines[i] = lines[i].Replace("H", "*");
            }
            if (lines[i].Contains('T'))
            {
                lines[i] = lines[i].Replace("T", "*");
            }
        }

        
        string updatedString = string.Join("\n", lines);

        return updatedString;
    }


    public string MoveHeadTailOnMap(string originalString, int row, int column, int moveNum, string uOrD, bool expand)
    {
        var lines = originalString.Split('\n');

        int newRow = row, newColumn = column;


        string logFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFiles");

        
        Directory.CreateDirectory(logFolderPath);

        
        string logFileName = "ApplicationLog.txt";
        string logFilePath = Path.Combine(logFolderPath, logFileName);


        switch (uOrD)
        {
            case "U":
                LogCommadIntoAFile("Up", moveNum, logFilePath);
                newRow = Math.Max(0, row - moveNum);
                break;
            case "D":
                LogCommadIntoAFile("Down", moveNum, logFilePath);
                newRow = Math.Min(lines.Length - 1, row + moveNum);
                break;
            case "L":
                LogCommadIntoAFile("Left", moveNum, logFilePath);
                newColumn = Math.Max(0, column - moveNum);
                break;
            case "R":
                LogCommadIntoAFile("Right", moveNum, logFilePath);
                newColumn = Math.Min(30, column + moveNum);
                break;
        }

        var line = "";
        var sb = new StringBuilder();

        if (newRow > 0 && newRow <= lines.Length-2)
        {

            if (expand)
            {
                switch (boostedNum)
                {
                    case 1:
                        if (previousCommand == "D" || previousCommand == "U")
                        {
                            if (previousCommand == "U")
                            {

                                if (newRow > 1)
                                {
                                    line = lines[newRow - 1].Trim();
                                    sb = new StringBuilder(line);

                                    int newPosition = newColumn - 1;
                                    sb[newPosition] = 'H';

                                    var line_ = ""; var line__ = "";
                                    var sb_ = new StringBuilder();
                                    var sb__ = new StringBuilder();


                                    line_ = lines[newRow].Trim();
                                    line__ = lines[newRow + 1].Trim();
                                    sb_ = new StringBuilder(line_);
                                    sb__ = new StringBuilder(line__);
                                    sb_[newPosition] = '0';
                                    sb__[newPosition] = 'T';
                                    lines[newRow] = "        " + sb_.ToString();
                                    lines[newRow + 1] = "        " + sb__.ToString();

                                    lines[newRow - 1] = "        " + sb.ToString();
                                }
                                else if (newRow <= 1)
                                {
                                    line = lines[newRow].Trim();
                                    sb = new StringBuilder(line);

                                    int newPosition = newColumn - 1;
                                    sb[newPosition] = 'H';

                                    var line_ = ""; var line__ = "";
                                    var sb_ = new StringBuilder();
                                    var sb__ = new StringBuilder();


                                    line_ = lines[newRow + 1].Trim();
                                    line__ = lines[newRow + 2].Trim();
                                    sb_ = new StringBuilder(line_);
                                    sb__ = new StringBuilder(line__);
                                    sb_[newPosition] = '0';
                                    sb__[newPosition] = 'T';
                                    lines[newRow + 1] = "        " + sb_.ToString();
                                    lines[newRow + 2] = "        " + sb__.ToString();

                                    lines[newRow] = "        " + sb.ToString();
                                }
                                else
                                {
                                    Console.Write("Failed to expand");
                                }
                            }
                            if (previousCommand == "D")
                            {


                                if (newRow > 1)
                                {
                                    line = lines[newRow + 1].Trim();
                                    sb = new StringBuilder(line);

                                    int newPosition = newColumn - 1;
                                    sb[newPosition] = 'H';

                                    var line_ = ""; var line__ = "";
                                    var sb_ = new StringBuilder();
                                    var sb__ = new StringBuilder();


                                    line_ = lines[newRow].Trim();
                                    line__ = lines[newRow + 1].Trim();
                                    sb_ = new StringBuilder(line_);
                                    sb__ = new StringBuilder(line__);
                                    sb_[newPosition] = '0';
                                    sb__[newPosition] = 'T';
                                    lines[newRow - 1] = "        " + sb__.ToString();
                                    lines[newRow] = "        " + sb_.ToString();
                                    lines[newRow + 1] = "        " + sb.ToString();
                                }
                                else if (newRow <= 1)
                                {
                                    line = lines[newRow].Trim();
                                    sb = new StringBuilder(line);

                                    int newPosition = newColumn - 1;
                                    sb[newPosition] = 'H';

                                    var line_ = ""; var line__ = "";
                                    var sb_ = new StringBuilder();
                                    var sb__ = new StringBuilder();


                                    line_ = lines[newRow + 1].Trim();
                                    line__ = lines[newRow + 2].Trim();
                                    sb_ = new StringBuilder(line_);
                                    sb__ = new StringBuilder(line__);
                                    sb_[newPosition] = '0';
                                    sb__[newPosition] = 'T';
                                    lines[newRow + 1] = "        " + sb_.ToString();
                                    lines[newRow + 2] = "        " + sb__.ToString();

                                    lines[newRow] = "        " + sb.ToString();
                                }
                                else
                                {
                                    Console.Write("Failed to expand");
                                }
                            }

                        }
                        else
                        {

                            line = lines[newRow].Trim();
                            sb = new StringBuilder(line);

                            if (newColumn > 0)
                            {


                                int newPosition = newColumn;
                                if (previousCommand == "R")
                                {
                                    newPosition = newColumn + 1;

                                    sb[newPosition] = 'H';
                                    sb[newColumn] = '0';
                                    sb[newColumn - 1] = 'T';
                                }
                                else if (previousCommand == "L")
                                {
                                    newPosition = newColumn + 1;

                                    sb[newColumn - 1] = 'H';
                                    sb[newColumn] = '0';
                                    sb[newPosition] = 'T';
                                }

                            }

                            else
                            {
                                int newPosition = newColumn;
                                if (previousCommand == "R")
                                {
                                    newPosition = newColumn + 1;
                                }
                                else if (previousCommand == "L")
                                {
                                    newPosition = newColumn + 1;
                                }

                                if (uOrD == "R")
                                {
                                    sb[newPosition] = 'H';
                                    sb[newColumn] = '0';
                                    sb[newPosition - 1] = 'T';
                                }

                                if (uOrD == "L")
                                {
                                    sb[newColumn-1] = 'H';
                                    sb[newColumn] = '0';
                                    sb[newPosition] = 'T';
                                }


                            }


                            lines[newRow] = "        " + sb.ToString();
                        }
                        break;

                    default:
                        Console.Write("Can not expand beyond this point.");
                        break;
                }


                LogCommadIntoAFile("Expand", moveNum, logFilePath);
            }

            else
            {
                line = lines[newRow].Trim();
                sb = new StringBuilder(line);

                if (uOrD == "D" || uOrD == "U")
                {

                    var line_ = "";
                    var sb_ = new StringBuilder();

                    int newPosition = newColumn - 1;
                    sb[newPosition] = 'H';

                    
                    CheckForBoosterObst(column, newColumn, line);

                    if (uOrD == "U")
                    {
                        line_ = lines[newRow + 1].Trim();
                        sb_ = new StringBuilder(line_);
                        sb_[newPosition] = 'T';
                        lines[newRow + 1] = "        " + sb_.ToString();
                    }

                    if (uOrD == "D")
                    {
                        line_ = lines[newRow - 1].Trim();
                        sb_ = new StringBuilder(line_);
                        sb_[newPosition] = 'T';
                        lines[newRow - 1] = "        " + sb_.ToString();
                    }

                }
                else
                {
                    if (newColumn > 0)
                    {
                        int newPosition = newColumn - 1;
                        sb[newPosition] = 'H';

                        if (uOrD == "R")
                        {
                            sb[newPosition - 1] = 'T';
                        }

                        if (uOrD == "L")
                        {
                            sb[newPosition + 1] = 'T';
                        }

                        
                        CheckForBoosterObst(column, newColumn, line);

                    }

                    else
                    {
                        int newPosition = newColumn;
                        sb[newPosition] = 'H';

                        if (!mapRunFirst)
                        {
                            if (uOrD == "R")
                            {
                                sb[newPosition - 1] = 'T';
                            }

                            if (uOrD == "L")
                            {
                                sb[newPosition + 1] = 'T';
                            }
                        }


                    }
                }


                lines[newRow] = "        " + sb.ToString();
            }

        }
        else
        {

            if (uOrD == "D" || uOrD == "U")
            {
                int targetRow = uOrD == "U" ? newRow + 2 : newRow - 2;
                if (targetRow >= 0 && targetRow < lines.Length)
                {
                    line = lines[row].Trim();
                    sb = new StringBuilder(line);

                    int newPosition = newColumn - 1;
                    sb[newPosition] = 'H';

                    
                    CheckForBoosterObst(column, newColumn, line);

                    var line_ = lines[targetRow].Trim();
                    var sb_ = new StringBuilder(line_);
                    sb_[newColumn - 1] = 'T';
                    lines[targetRow] = "        " + sb_.ToString();
                }
            }


            else
            {
                line = lines[row].Trim();
                sb = new StringBuilder(line);

                if (newColumn > 0)
                {
                    int newPosition = newColumn - 1;
                    sb[newPosition] = 'H';

                    if (!mapRunFirst)
                    {
                        int tPosition = uOrD == "R" ? newPosition - 1 : newPosition + 1;
                        sb[tPosition] = 'T';
                    }

                    
                    CheckForBoosterObst(column, newColumn, line);
                }
                else
                {
                    int newPosition = newColumn;
                    sb[newPosition] = 'H';

                    if (!mapRunFirst)
                    {
                        int tPosition = uOrD == "R" ? newPosition - 1 : newPosition + 1;
                        sb[tPosition] = 'T';
                    }
                }

            }

            lines[row] = "        " + sb.ToString();
        }

        return string.Join("\n", lines);

    }


    public int boostedNum = 0, boosterCount = 0;
    public string previousCommand = "";
    public void CheckForBoosterObst(int column, int newColumn, string line)
    {
        
        for (int i = Math.Min(column, newColumn) - 1; i < Math.Max(column, newColumn); i++)
        {
            if (line[i] == '#')
            {
                Environment.Exit(0);
            }
            else if (line[i] == 'B')
            {
                boosterCount++;
                line = line.Replace('B', '*');

            }
        }

    }


    public void LogCommadIntoAFile(string input, int move, string logFilePath)
    {
        using (StreamWriter logFileWriter = new StreamWriter(logFilePath, append: true))
        {
            
            string logMessage = $"Command: {input} and move: {move}, was executed.";

            logFileWriter.WriteLine(logMessage);
        }

    }
    public static (int, int) LocateCatapilHead(string originalString)
    {

        var lines = originalString.Split('\n');


        int currentRow = 0;
        int currentColumn = 0;


        foreach (string line in lines)
        {

            if (line.Contains("H"))
            {

                string trimmedLine = line.Trim();


                int kIndex = trimmedLine.IndexOf("H");


                currentColumn = kIndex != -1 ? kIndex + 1 : -1;


                break;
            }


            currentRow++;
        }


        return (currentRow, currentColumn);
    }



}





