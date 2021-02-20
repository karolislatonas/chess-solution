namespace Chess.UseCases
{
    public abstract class CommandHandlerBase<TCommand>
    {
        public abstract void ExecuteCommand(TCommand command);
    }
}
