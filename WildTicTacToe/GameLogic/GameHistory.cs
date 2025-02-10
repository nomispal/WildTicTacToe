using System.Collections.Generic;
using WildTicTacToe.Models;

namespace WildTicTacToe.GameLogic
{
    public class GameHistory
    {
        private readonly Stack<Move> _undoStack = new();
        private readonly Stack<Move> _redoStack = new();

        public void Restore(List<Move> moves)
        {
            _undoStack.Clear();
            _redoStack.Clear();
            foreach (var move in moves)
            {
                _undoStack.Push(move);
            }
        }

        public List<Move> GetAllMoves()
        {
            return _undoStack.ToList();
        }
        public void AddMove(Move move) => _undoStack.Push(move);


        public Move UndoMove()
        {
            var move = _undoStack.Pop();
            _redoStack.Push(move);
            return move;
        }

        public Move RedoMove()
        {
            var move = _redoStack.Pop();
            _undoStack.Push(move);
            return move;
        }

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;
    }
}