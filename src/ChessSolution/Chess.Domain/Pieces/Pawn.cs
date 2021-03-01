using Chess.Domain.Extensions;
using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Pawn")]
    public class Pawn : PieceBase
    {
        public Pawn(ChessColor color) : base(color)
        {
            TakeDirections = EnumerateTakeDirections().ToArray();
            MoveDirections = EnumerateMoveDirections().ToArray();
        }

        public int StartingRow => Color.IsWhite() ? 2 : 7;

        public int RowMoveDirection => Color.IsWhite() ? 1 : -1;

        public override IMover Mover => new ComposedMover(new PawnMover(), new PawnTakeMover());

        public override IReadOnlyList<Location> MoveDirections { get; }

        public override IReadOnlyList<Location> TakeDirections { get; }

        private IEnumerable<Location> EnumerateMoveDirections()
        {
            yield return new Location(0, RowMoveDirection);
        }

        private IEnumerable<Location> EnumerateTakeDirections()
        {
            yield return new Location(-1, RowMoveDirection);
            yield return new Location(1, RowMoveDirection);
        }
    }
}
