using System.Text.Json;
using System.Text.Json.Serialization;
using WildTicTacToe.Models;

namespace WildTicTacToe.GameLogic
{
    public class GameManager
    {
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public void SaveGame(Game game)
        {
            var state = new GameState
            {
                Board = ConvertToJaggedArray(game.GetBoardState()),
                CurrentPlayerIndex = game.GetCurrentPlayerIndex(),
                History = game.GetMoveHistory(),
                Player1Type = game.GetPlayer1() is HumanPlayer ? PlayerType.Human : PlayerType.AI,
                Player2Type = game.GetPlayer2() is HumanPlayer ? PlayerType.Human : PlayerType.AI,
                Player1Symbol = game.GetPlayer1().Symbol,
                Player2Symbol = game.GetPlayer2().Symbol
            };

            File.WriteAllText("savegame.json", JsonSerializer.Serialize(state, _options));
        }

        public Game LoadGame()
        {
            if (!File.Exists("savegame.json")) return null;

            var json = File.ReadAllText("savegame.json");
            var state = JsonSerializer.Deserialize<GameState>(json, _options);

            var player1 = CreatePlayer(state.Player1Type, state.Player1Symbol);
            var player2 = CreatePlayer(state.Player2Type, state.Player2Symbol);

            var game = new Game(player1, player2)
            {
                CurrentPlayerIndex = state.CurrentPlayerIndex
            };

            game.RestoreState(ConvertTo2DArray(state.Board), state.History);
            return game;
        }
        private char[][] ConvertToJaggedArray(char[,] board)
        {
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);
            char[][] jaggedArray = new char[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new char[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = board[i, j];
                }
            }

            return jaggedArray;
        }

        private char[,] ConvertTo2DArray(char[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;
            char[,] board = new char[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    board[i, j] = jaggedArray[i][j];
                }
            }

            return board;
        }


        private Player CreatePlayer(PlayerType type, char symbol)
        {
            Player player = type switch
            {
                PlayerType.Human => new HumanPlayer(),
                PlayerType.AI => new AIPlayer(),
                _ => throw new InvalidOperationException("Unknown player type")
            };

            player.Symbol = symbol;
            return player;
        }



        public class GameState
        {
            public char[][] Board { get; set; }
            public int CurrentPlayerIndex { get; set; }
            public List<Move> History { get; set; }
            public PlayerType Player1Type { get; set; }
            public PlayerType Player2Type { get; set; }
            public char Player1Symbol { get; set; }
            public char Player2Symbol { get; set; }
        }

        public enum PlayerType { Human, AI }
    }

}