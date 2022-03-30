class Program
{
    static string[,] SetUpMaze(bool isReal)
    {
        string[,] maze = new string[8, 8];
        
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                maze[i, j] = "?";
            }
        }
        
        if (isReal) maze = AddTheWumpus(maze);
        return maze;
    }
    
    static string[,] AddTheWumpus(string[,] maze) {
        Random r = new Random();
        int locX = r.Next(8);
        int locY = r.Next(8);
        maze[locX, locY] = "&";
        
        maze = CreateEvidence(maze, locX, locY);
        return maze; 
    }
    
    static string[,] CreateEvidence(string[,] maze, int locX, int locY) {
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) {
                if (Math.Abs(x - locX) == 1 && Math.Abs(y - locY) == 1) {
                    maze[x, y] = "!";
                } else if (Math.Abs(x - locX) == 0 && Math.Abs(y - locY) == 1) {
                    maze[x, y] = "!";
                } else if (Math.Abs(x - locX) == 1 && Math.Abs(y - locY) == 0) {
                    maze[x, y] = "!";
                }
            }
        }
        
        return maze;
    }
    
    static void PrintMaze(string[,] maze) {
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Console.Write(maze[x,y] + " ");
            }
            Console.WriteLine();
        }
    }

    static void CheckForEvidence(string dir, string[,] userMaze, string[,] maze, int[] userPos)
    {
        if (maze[userPos[0], userPos[1]] == "!") {
            userMaze[userPos[0], userPos[1]] = "^";
        } else if (maze[userPos[0], userPos[1]] == "&") {
            Console.WriteLine("You encountered the Wumpus!");
            Console.WriteLine("Game Over.");
        } else {
            userMaze[userPos[0], userPos[1]] = "*";
        }

        SetLastPosition(userMaze, maze, userPos);
    }

    static void SetLastPosition(string[,] userMaze, string[,] maze, int[] userPos)
    {
        
    }
    
    static void Main()
    {
        string[,] maze = SetUpMaze(true);
        bool gameOver = false;


        string[,] userMaze = SetUpMaze(false);
        int[] userPos = {3, 7};
        userMaze[userPos[0], userPos[1]] = "*";
        
        
        while (!gameOver)
        {
            PrintMaze(userMaze);
            Console.Write("Where would you like to go?");
            string dir = Console.ReadLine();

            switch (dir)
            {
                case "n": case "north":
                    userPos[1]--;
                    CheckForEvidence("north", userMaze, maze, userPos);
                    break;
                case "s": case "south":
                    userPos[1]++;
                    CheckForEvidence("south", userMaze, maze, userPos);
                    break;
                case "e": case "east":
                    userPos[0]++;
                    CheckForEvidence("east", userMaze, maze, userPos);
                    break;
                case "w": case "west":
                    userPos[0]--;
                    CheckForEvidence("west", userMaze, maze, userPos);
                    break;
                case "arrow": case "shoot":
                    break;
                default:
                    Console.WriteLine("That's not a direction!");
                    break;
            }
        }
    }
}

/*
    ? - Unknown location
    0 - Traveled location, no evidence
    ! - Traveled location, evidence
    * - Current location, no evidence
    ^ - Current location, evidence
    & - Wumpus location
*/