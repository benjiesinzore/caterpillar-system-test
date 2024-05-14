

var callMap = new GECA();
Console.WriteLine("\nSpice Map:");
callMap.DisplayMap();


while (true)
{
    Console.WriteLine("\nEnter command (U/D/L/R/UN/RD/MP/EX):");
    string input = Console.ReadLine().ToUpper();

    switch (input)
    {
        case "EX":
            if (callMap.boosterCount > 0)
            {
                callMap.boostedNum++;
                callMap.UpdateMap(0, input, true, false);
            }
            else
            {
                Console.Write("There is no Booters.");
            }
            break;
        case "MP":
            callMap.DisplayMap();
            break;
        case "UN":
            callMap.UndoCommand();
            break;
        case "U":
        case "D":
        case "L":
        case "R":
            Console.WriteLine("Enter the number of moves:");
            if (int.TryParse(Console.ReadLine(), out int n))
            {
                callMap.UpdateMap(n, input, false, false);

                callMap.previousCommand = input;
            }
            else
            {
                Console.WriteLine("Invalid input for number of moves.");
            }
            break;
        default:
            Console.WriteLine("Invalid command. Please enter U, D, L, or R.");
            break;
    }


    Console.WriteLine("\nRader Image:\n");
    callMap.ReduceRowsTo11();

}

