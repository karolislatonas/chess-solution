using Chess.Domain.Movement.Moves;

namespace Chess.Domain.Movement
{
    public class MoveSequenceItem
    {
        public MoveSequenceItem(int sequenceNumber, IMove move)
        {
            SequenceNumber = sequenceNumber;
            Move = move;
        }

        public int SequenceNumber { get; }

        public IMove Move { get; }
    }
}
