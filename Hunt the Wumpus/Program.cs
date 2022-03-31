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
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
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
            SetLastPosition(dir, userMaze, maze, userPos);
            userMaze[userPos[0], userPos[1]] = "^";
        } else if (maze[userPos[0], userPos[1]] == "&") {
            Console.WriteLine("You encountered the Wumpus!");
            Console.WriteLine("Game Over.");
        } else {
            SetLastPosition(dir, userMaze, maze, userPos);
            userMaze[userPos[0], userPos[1]] = "*";
        }
    }

    static void SetLastPosition(string dir, string[,] userMaze, string[,] maze, int[] userPos)
    {
        switch (dir)
        {
            case "north":
                if (maze[userPos[0] + 1, userPos[1]] == "!") userMaze[userPos[0] + 1, userPos[1]] = "!";
                else userMaze[userPos[0] + 1, userPos[1]] = "0";
                break;
            case "south":
                if (maze[userPos[0] - 1, userPos[1]] == "!") userMaze[userPos[0] - 1, userPos[1]] = "!";
                else userMaze[userPos[0] - 1, userPos[1]] = "0";
                break;
            case "east":
                if (maze[userPos[0], userPos[1] - 1] == "!") userMaze[userPos[0], userPos[1] - 1] = "!";
                else userMaze[userPos[0], userPos[1] - 1] = "0";
                break;
            case "west":
                if (maze[userPos[0], userPos[1] + 1] == "!") userMaze[userPos[0], userPos[1] + 1] = "!";
                else userMaze[userPos[0], userPos[1] + 1] = "0";
                break;
        }
    }

    static void CheckForArrow(string arrowDir, int[] userPos, string[,] maze)
    {
        bool won = false;
        
        switch (arrowDir)
        {
            case "north": case "n":
                for (int i = 0; i < 8; i++)
                {
                    if (maze[i, userPos[1]] == "&" && i < userPos[0]) {
                        PrintMaze(maze);
                        Console.WriteLine("Congratulations! You hit the Wumpus!");
                        won = true;
                    }
                }
                break;
            case "south": case "s":
                for (int i = 0; i < 8; i++)
                {
                    if (maze[i, userPos[1]] == "&" && i > userPos[0]) {
                        PrintMaze(maze);
                        Console.WriteLine("Congratulations! You hit the Wumpus!");
                        won = true;
                    } 
                }
                break;
            case "east": case "e":
                for (int i = 0; i < 8; i++)
                {
                    if (maze[userPos[0], i] == "&" && i > userPos[1]) {
                        PrintMaze(maze);
                        Console.WriteLine("Congratulations! You hit the Wumpus!");
                        won = true;
                    } 
                }
                break;
            case "west": case "w":
                for (int i = 0; i < 8; i++)
                {
                    if (maze[userPos[0], i] == "&" && i < userPos[1]) {
                        PrintMaze(maze);
                        Console.WriteLine("Congratulations! You hit the Wumpus!");
                        won = true;
                    } 
                }
                break;
        }

        if (!won)
        {
            PrintMaze(maze);
            Console.WriteLine("You lost ...");
        }
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
                    userPos[0]--;
                    CheckForEvidence("north", userMaze, maze, userPos);
                    break;
                case "s": case "south":
                    userPos[0]++;
                    CheckForEvidence("south", userMaze, maze, userPos);
                    break;
                case "e": case "east":
                    userPos[1]++;
                    CheckForEvidence("east", userMaze, maze, userPos);
                    break;
                case "w": case "west":
                    userPos[1]--;
                    CheckForEvidence("west", userMaze, maze, userPos);
                    break;
                case "arrow": case "shoot":
                    Console.WriteLine("Which direction do you want to shoot the arrow?");
                    string arrowDir = Console.ReadLine();
                    CheckForArrow(arrowDir, userPos, maze);
                    gameOver = true;
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