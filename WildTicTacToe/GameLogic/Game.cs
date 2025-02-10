using System;
using System.Linq;
using System.Threading;
using WildTicTacToe.Models;
using WildTicTacToe.Strategies;

namespace WildTicTacToe.GameLogic
{
    public class Game
    {
        private readonly Board _board = new();
        private readonly GameHistory _history = new();
        private readonly Player[] _players;
        private int _currentPlayerIndex;
        private readonly GameManager _gameManager = new();

        public Game(Player player1, Player player2)
        {
            _players = new[] { player1, player2 };
            _players[0].Symbol = 'X';
            _players[1].Symbol = 'O';
        }

        public void Start()
        {
            while (true)
            {
                Console.Clear();
                DisplayBoard();

                if (CheckGameOver())
                {
                    DisplayFinalResult();
                    break;
                }

                var currentPlayer = _players[_currentPlayerIndex];

                try
                {
                    Move move;
                    if (currentPlayer.MoveStrategy is HumanStrategy)
                    {
                        Console.WriteLine($"Player {currentPlayer.Symbol}'s turn (row,col): ");
                        var input = Console.ReadLine()?.Trim().ToLower() ?? "";

                        if (HandleCommand(input)) continue;

                        if (!TryParseMove(input, out move))
                            throw new ArgumentException("Invalid format. Use 'row,col' (0-2)");
                    }
                    else
                    {
                        
                        move = currentPlayer.MoveStrategy.GenerateMove(_board, currentPlayer);
                        Console.WriteLine($"AI ({currentPlayer.Symbol}) played at: {move.Position.X},{move.Position.Y}");
                        Thread.Sleep(1000); 
                    }

                    ProcessPlayerMove(currentPlayer, move);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Thread.Sleep(1500);
                }
            }
        }

        private void ProcessPlayerMove(Player player, Move move)
        {
            move.Player = player;

            if (!_board.IsValidMove(move))
                throw new InvalidOperationException("Position already occupied");

            move.Execute(_board);
            _history.AddMove(move);
            _currentPlayerIndex = (_currentPlayerIndex + 1) % 2;
        }

        private bool HandleCommand(string input)
        {
            switch (input)
            {
                case "undo" when _history.CanUndo:
                    var undoMove = _history.UndoMove();
                    undoMove.Undo(_board);
                    return true;

                case "redo" when _history.CanRedo:
                    var redoMove = _history.RedoMove();
                    redoMove.Execute(_board);
                    return true;

                case "save":
                    SaveGame();
                    return true;

                case "load":
                    LoadGame();
                    return true;

                case "help":
                    ShowHelp();
                    return true;

                case "exit":
                    Environment.Exit(0);
                    return true;

                default:
                    return false;
            }
        }

        private bool TryParseMove(string input, out Move move)
        {
            move = null;
            var parts = input.Split(',');
            if (parts.Length != 2) return false;

            if (int.TryParse(parts[0], out int row) &&
                int.TryParse(parts[1], out int col) &&
                row >= 0 && row < Board.Size &&
                col >= 0 && col < Board.Size)
            {
                move = new Move
                {
                    Position = new Position(row, col),
                    Timestamp = DateTime.Now
                };
                return true;
            }
            return false;
        }

        private bool CheckGameOver()
        {
            return TicTacToeRules.CheckWin(_board, _players[0]) ||
                   TicTacToeRules.CheckWin(_board, _players[1]) ||
                   _board.IsFull();
        }

        private void DisplayBoard()
        {
            for (var i = 0; i < Board.Size; i++)
            {
                Console.WriteLine(string.Join("|", _board.GetRow(i)
                    .Select(c => c == '\0' ? ' ' : c)));
                if (i < Board.Size - 1) Console.WriteLine("-----");
            }
        }

        private void DisplayFinalResult()
        {
            Console.Clear();
            DisplayBoard();
            Console.WriteLine(_board.IsFull()
                ? "Game over! It's a draw!"
                : $"Player {_players[(_currentPlayerIndex + 1) % 2].Symbol} wins!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void SaveGame()
        {
            _gameManager.SaveGame(this);
            Console.WriteLine("Game saved!");
            Thread.Sleep(1000);
        }

        private void LoadGame()
        {
            var loadedGame = _gameManager.LoadGame();
            if (loadedGame == null)
            {
                Console.WriteLine("No saved game found!");
                Thread.Sleep(1000);
                return;
            }

            Console.WriteLine("Game loaded!");
            Thread.Sleep(1000);
            loadedGame.Start();
            Environment.Exit(0);
        }

        private static void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("=== COMMANDS ===");
            Console.WriteLine("undo    - Revert last move");
            Console.WriteLine("redo    - Restore undone move");
            Console.WriteLine("save    - Save current game");
            Console.WriteLine("load    - Load saved game");
            Console.WriteLine("help    - Show this message");
            Console.WriteLine("exit    - Quit to main menu");
            Console.WriteLine("\nPress any key to continue...");
            Thread.Sleep(2500);
        }

        // Save/Load infrastructure
        public Player GetPlayer1() => _players[0];
        public Player GetPlayer2() => _players[1];
        public char[,] GetBoardState() => _board.GetGrid();
        public int GetCurrentPlayerIndex() => _currentPlayerIndex;
        public List<Move> GetMoveHistory() => _history.GetAllMoves();

        public int CurrentPlayerIndex
        {
            get => _currentPlayerIndex;
            set => _currentPlayerIndex = value;
        }

        public void RestoreState(char[,] boardState, List<Move> history)
        {
            _board.Restore(boardState);
            _history.Restore(history);

            // Restore player references in moves
            foreach (var move in history)
            {
                move.Player = move.PlayerSymbol == _players[0].Symbol
                    ? _players[0]
                    : _players[1];
                move.PlayerSymbol = move.Player.Symbol;
            }
        }

        public static Game Load() => new GameManager().LoadGame();
        public void Save() => _gameManager.SaveGame(this);
    }
}
