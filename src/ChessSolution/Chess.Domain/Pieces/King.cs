using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("King")]
    public class King : PieceBase
    {
        private readonly static IReadOnlyList<Location> MoveDirectionsA = new[] {
            new Location(1, 1),
            new Location(-1, -1),
            new Location(-1, 1),
            new Location(1, -1),
            new Location(1, 0),
            new Location(0, 1),
            new Location(-1, 0),
            new Location(0, -1)
        };

        public King(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new SingleJumpMover();

        public override IReadOnlyList<Location> MoveDirections => MoveDirectionsA;

        public override IReadOnlyList<Location> TakeDirections => MoveDirectionsA;
    }
}
