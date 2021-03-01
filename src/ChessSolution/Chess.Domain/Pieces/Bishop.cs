using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Bishop")]
    public class Bishop : PieceBase
    {
        private readonly static IReadOnlyList<Location> PossibleMoveDirections = new[] {
            new Location(1, 1),
            new Location(-1, -1),
            new Location(-1, 1),
            new Location(1, -1)
        };

        public Bishop(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new DirectionalMover();

        public override IReadOnlyList<Location> MoveDirections => PossibleMoveDirections;

        public override IReadOnlyList<Location> TakeDirections => PossibleMoveDirections;
    }
}
