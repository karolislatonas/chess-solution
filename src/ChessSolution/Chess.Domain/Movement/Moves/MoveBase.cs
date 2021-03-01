namespace Chess.Domain.Movement.Moves
{
    public abstract class MoveBase : IMove
    {
        protected readonly Location from;
        protected readonly Location to;

        public MoveBase(Location from, Location to)
        {
            this.from = from;
            this.to = to;
        }

        public abstract void ApplyChanges(Board board);
    }
}
