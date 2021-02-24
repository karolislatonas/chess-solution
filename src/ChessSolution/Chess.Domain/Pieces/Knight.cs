using Chess.Domain.Movement;
using Chess.Domain.Movement.Movers;
using System.Collections.Generic;
using System.Diagnostics;

namespace Chess.Domain.Pieces
{
    [DebuggerDisplay("Knight")]
    public class Knight : PieceBase
    {
        private readonly static IReadOnlyList<Location> MoveDirectionsA = new[] {
           new Location(2, 1),
           new Location(2, -1),
           new Location(-2, 1),
           new Location(-2, -1),
           new Location(1, 2),
           new Location(-1, 2),
           new Location(1, -2),
           new Location(-1, -2)
        };

        public Knight(ChessColor color) : base(color)
        {
        }

        public override IMover Mover => new SingleJumpMover();

        public override IReadOnlyList<Location> MoveDirections => MoveDirectionsA;
                                                                  
        public override IReadOnlyList<Location> TakeDirections => MoveDirectionsA;
    }
}
