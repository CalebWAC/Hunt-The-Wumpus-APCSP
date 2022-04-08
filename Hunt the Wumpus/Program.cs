using System;

class Program
{
    // Condition for if the game is won
    static bool won = false;
    
    /** Creates and fills a maze with "?".
    Also adds the Wumpus if it is not the user maze */
    static string[,] SetUpMaze(bool isReal)
    {
        string[,] maze = new string[8, 8];
        
        // For each x position and y position, add a ? to the maze
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                maze[i, j] = "?";
            }
        }
        
        // Adds the Wumpus to the maze if the maze is the real one and not the user one
        if (isReal) maze = AddTheWumpus(maze);
        return maze;
    }
    
    /** Inserts the Wumpus "&" at a random location in the maze */
    static string[,] AddTheWumpus(string[,] maze) {
        // Randomizes the position of the Wumpus
        Random r = new Random();
        int locX = r.Next(8);
        int locY = r.Next(8);
        
        // Adds the Wumpus and the evidence to the map
        maze[locX, locY] = "&";
        maze = CreateEvidence(maze, locX, locY);
        
        return maze; 
    }
    
    /** Adds a "!" to each space surrounding the wumpus*/
    static string[,] CreateEvidence(string[,] maze, int locX, int locY) {
        // Loops over each x and y position pair
        for (int x = 0; x < 8; x++) {
            for (int y = 0; y < 8; y++) { 
                if (Math.Abs(x - locX) == 1 && Math.Abs(y - locY) == 1) {
                    // Checks if the current x and y position are within 1 from the Wumpus
                    maze[x, y] = "!";
                } else if (Math.Abs(x - locX) == 0 && Math.Abs(y - locY) == 1) {
                    // Checks if the current x position is the same as the Wumpus,
                    // but the y position is within 1 space
                    maze[x, y] = "!";
                } else if (Math.Abs(x - locX) == 1 && Math.Abs(y - locY) == 0) {
                    // Checks if the current x position is within 1 space from the Wumpus
                    // and if the y position is the same
                    maze[x, y] = "!";
                }
            }
        }

        maze = AddObstacles(maze, locX, locY);
        return maze;
    }

    static string[,] AddObstacles(string[,] maze, int wX, int wY)
    {
        var r = new Random();
        for (int i = 0; i < 5; i++)
        {
            int locX = r.Next(8);
            int locY = r.Next(8);
            if (locX != wX && locY != wY) maze[locX, locY] = "@";
            Console.WriteLine($"Obstacle at {locX}, {locY}");
        }

        return maze;
    }
    
    /** Formats and prints the maze to the console */
    static void PrintMaze(string[,] maze) {
        // Adds new lines
        Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
        
        // Loops over position in the maze using x and y coordinates
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                switch (maze[x, y]) // Checks the current character at (x, y)
                {
                    case "*": case "^": // Turns the text blue if it represents the player
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.BackgroundColor = ConsoleColor.White;
                        break;
                    case "0": // Turns the text dark yellow if it represents a travelled location
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.DarkYellow; break;
                    case "!": // Turns the text red if it represents one space from the Wumpus
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Red; break;
                    case "&": // Turns the text magenta if it represents the Wumpus
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Magenta; break;
                    case "?": // Turns the text white if it represents an untravelled location
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White; break;
                    case "@": // Turns the text yellow if it represents an obstacle
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                }
                
                Console.Write(maze[x,y] + " "); 
            }
            Console.WriteLine();
            
            // Resets the console color
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    /** Checks to see if the player is at evidence or the Wumpus */
    static void CheckForEvidence(string dir, string[,] userMaze, string[,] maze, int[] userPos)
    {
        if (maze[userPos[0], userPos[1]] == "!") {
            // Changes the player's character to ^ if the current location has evidence (!)
            SetLastPosition(dir, userMaze, maze, userPos);
            userMaze[userPos[0], userPos[1]] = "^";
        } else if (maze[userPos[0], userPos[1]] == "&") {
            // Ends the game if the player is on the same spot as the Wumpus
            PrintMaze(maze);
            Console.WriteLine("You encountered the Wumpus!");
            Console.WriteLine("Game Over.");
            Environment.Exit(1);
        } else if (maze[userPos[0], userPos[1]] == "@") {
            switch (dir)
            {
                case "north": 
                    userPos[0]++;
                    userMaze[userPos[0] - 1, userPos[1]] = "@";
                    break;
                case "south": 
                    userPos[0]--; 
                    userMaze[userPos[0] + 1, userPos[1]] = "@";
                    break;
                case "east": 
                    userPos[1]--; 
                    userMaze[userPos[0], userPos[1] + 1] = "@";
                    break;
                case "west": 
                    userPos[1]++; 
                    userMaze[userPos[0], userPos[1] - 1] = "@";
                    break;
            }
        } else {
            // If the space is normal, change the user's last position
            SetLastPosition(dir, userMaze, maze, userPos);
            userMaze[userPos[0], userPos[1]] = "*";
        }
    }

    /** Changes the character at the position where the user was previously at */
    static void SetLastPosition(string dir, string[,] userMaze, string[,] maze, int[] userPos)
    {
        // Checks for which direction the user came from
        switch (dir)
        {
            case "north": // Checks if the previous location in maze was "!", otherwise sets it to "0"
                if (maze[userPos[0] + 1, userPos[1]] == "!") userMaze[userPos[0] + 1, userPos[1]] = "!";
                else userMaze[userPos[0] + 1, userPos[1]] = "0";
                break;
            case "south": // Checks if the previous location in maze was "!", otherwise sets it to "0"
                if (maze[userPos[0] - 1, userPos[1]] == "!") userMaze[userPos[0] - 1, userPos[1]] = "!";
                else userMaze[userPos[0] - 1, userPos[1]] = "0";
                break;
            case "east": // Checks if the previous location in maze was "!", otherwise sets it to "0"
                if (maze[userPos[0], userPos[1] - 1] == "!") userMaze[userPos[0], userPos[1] - 1] = "!";
                else userMaze[userPos[0], userPos[1] - 1] = "0";
                break;
            case "west": // Checks if the previous location in maze was "!", otherwise sets it to "0"
                if (maze[userPos[0], userPos[1] + 1] == "!") userMaze[userPos[0], userPos[1] + 1] = "!";
                else userMaze[userPos[0], userPos[1] + 1] = "0";
                break;
        }
    }

    /** Checks if the direction and position the user shot the arrow hits the Wumpus */
    static void CheckForArrow(string arrowDir, int[] userPos, string[,] maze)
    {
        // Checks which direction the user shot the arrow
        switch (arrowDir) 
        {
            case "north": case "n": 
                // If the Wumpus is at the same y position and in line of the arrow, the user wins
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
                // If the Wumpus is at the same y position and in line of the arrow, the user wins
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
                // If the Wumpus is at the same x position and in line of the arrow, the user wins
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
                // If the Wumpus is at the same y position and in line of the arrow, the user wins
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

        // If the arrow missed, the correct location and "You lost ..." are outputted
        if (!won)
        {
            PrintMaze(maze);
            Console.WriteLine("You lost ...");
        }
    }

    private static Random r = new Random();
    
    static void Main()
    {
        // Sets up the real maze, sets won to false, and changes the background color
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        string[,] maze = SetUpMaze(true);
        bool won = false;

        // Sets up the maze that the user sees and the user's positiion
        string[,] userMaze = SetUpMaze(false);
        
        // int[] userPos = {r.Next(8), r.Next(8)};
        int[] userPos = {3, 7};
        userMaze[userPos[0], userPos[1]] = "*";
        
        
        while (!won)
        {
            // Asks the user where they want to go next
            PrintMaze(userMaze);
            Console.Write("Where would you like to go?");
            string dir = Console.ReadLine();

            // Checks which direction the user inputted, then changes the user's position
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
                    // Shoots the arrow based on the user's direction and ends the game
                    Console.WriteLine("Which direction do you want to shoot the arrow?");
                    string arrowDir = Console.ReadLine();
                    CheckForArrow(arrowDir, userPos, maze);
                    won = true;
                    break;
                default: // Runs if the user did not input a valid direction or command
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