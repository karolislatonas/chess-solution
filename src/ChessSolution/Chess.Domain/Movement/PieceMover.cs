using System.Collections.Generic;

namespace Chess.Domain.Movement
{
    public class PieceMover
    {
        private readonly Board board;

        public PieceMover(Board board)
        {
            this.board = board;
        }

        public void EnsureIsValidMove(Location from, Location to)
        {
            
        }

        public HashSet<Location> GetAvailableMoves(Location from)
        {
            var piece = board.GetPieceAt(from);

            return new HashSet<Location>();
        }
    }
}
