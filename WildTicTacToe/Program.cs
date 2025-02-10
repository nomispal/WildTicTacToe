using WildTicTacToe.Models;
using WildTicTacToe.Strategies;
using WildTicTacToe.GameLogic;

var gameManager = new GameManager();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== WILD TIC-TAC-TOE ===");
    Console.WriteLine("1. Human vs Human");
    Console.WriteLine("2. Human vs Computer");
    Console.WriteLine("3. Load Saved Game");
    Console.WriteLine("4. Help");
    Console.WriteLine("5. Exit");
    Console.Write("Select an option: ");

    var input = Console.ReadLine();

    switch (input)
    {
        case "1":
            StartGame(new HumanStrategy(), new HumanStrategy());
            break;
        case "2":
            StartGame(new HumanStrategy(), new AIStrategy());
            break;
        case "3":
            LoadGame();
            break;
        case "4":
            ShowHelp();
            break;
        case "5":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Invalid option! Press any key to continue...");
            Console.ReadKey();
            break;
    }
}

void StartGame(MoveStrategy player1Strategy, MoveStrategy player2Strategy)
{
    var player1 = new HumanPlayer { Symbol = 'X', MoveStrategy = player1Strategy };

    Player player2;
    if (player2Strategy is AIStrategy)  // Ensure AI is properly assigned
    {
        player2 = new AIPlayer { Symbol = 'O', MoveStrategy = player2Strategy };
    }
    else
    {
        player2 = new HumanPlayer { Symbol = 'O', MoveStrategy = player2Strategy };
    }

    var game = new Game(player1, player2);
    game.Start();

    Console.WriteLine("Game over! Press any key to return to menu...");
    Console.ReadKey();
}



void LoadGame()
{
    try
    {
        var game = gameManager.LoadGame();
        if (game == null)
        {
            Console.WriteLine("No saved game found!");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("Game loaded successfully!");
        game.Start();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading game: {ex.Message}");
        Console.ReadKey();
    }
}

void ShowHelp()
{
    Console.Clear();
    Console.WriteLine("=== GAME HELP ===");
    Console.WriteLine("How to Play:");
    Console.WriteLine("- Enter coordinates as 'row,col' (0-2)");
    Console.WriteLine("- Example: '1,1' for center position");
    Console.WriteLine("\nGame Commands:");
    Console.WriteLine("undo    - Revert last move");
    Console.WriteLine("redo    - Restore undone move");
    Console.WriteLine("save    - Save game state");
    Console.WriteLine("load    - Load saved game");
    Console.WriteLine("help    - Show commands");
    Console.WriteLine("exit    - Quit to main menu");
    Console.WriteLine("\nPress any key to return to menu...");
    Console.ReadKey();
}