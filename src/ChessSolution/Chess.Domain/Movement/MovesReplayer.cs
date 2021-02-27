namespace Chess.Domain.Movement
{
    public class MovesReplayer
    {
        private readonly MovesLog movesLog;
        private int currentMoveNumber;

        public MovesReplayer(MovesLog movesLog, int moveNumber = 0)
        {   
            this.movesLog = movesLog;

            Reset();

            ToMove(moveNumber);
        }

        public Board Board { get; private set; }

        public void ToPreviousMove()
        {
            ToMove(PreviousMoveNumber());
        }

        public void ToNextMove()
        {
            ToMove(NextMoveNumber());
        }

        public void ToLastMove()
        {
            var lastMoveNumber = movesLog.LatestMove.SequenceNumber;
            ToMove(lastMoveNumber);
        }

        public void ToMove(int sequenceNumber)
        {
            if (sequenceNumber > currentMoveNumber)
                ApplyMoves(NextMoveNumber(), sequenceNumber);

            if (sequenceNumber < currentMoveNumber)
            {
                Reset();
                ApplyMoves(currentMoveNumber, sequenceNumber);
            }
        }

        private int PreviousMoveNumber() => currentMoveNumber - 1;

        private int NextMoveNumber() => currentMoveNumber + 1;

        public void Reset()
        {
            currentMoveNumber = 0;
            Board = new Board();
        }

        private void ApplyMoves(int fromSequenceNumber, int toSequenceNumber)
        {
            var moves = movesLog.GetMovesFromTo(fromSequenceNumber, toSequenceNumber);

            foreach(var move in moves)
            {
                currentMoveNumber = move.SequenceNumber;
                Board.ApplyMove(move);
            }
        }
    }
}
