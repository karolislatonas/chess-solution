using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Queen")]
    public class Queen : PieceBase
    {
        private readonly static IReadOnlyList<Location> MoveDirectionsA = new[] {
            new Location(1, 1),
            new Location(-1, -1),
            new Location(-1, 1),
            new Location(1, -1),
            new Location(1, 0),
            new Location(0, 1),
            new Location(-1, 0),
            new Location(0, -1),
        };

        public Queen(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new DirectionalMover();

        public override IReadOnlyList<Location> MoveDirections => MoveDirectionsA;
                                                                  
        public override IReadOnlyList<Location> TakeDirections => MoveDirectionsA;
    }
}
