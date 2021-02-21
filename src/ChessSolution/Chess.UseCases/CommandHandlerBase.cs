namespace Chess.UseCases
{
    public abstract class CommandHandlerBase<TCommand>
    {
        public abstract void ExecuteCommand(TCommand command);
    }

    public abstract class CommandHandlerBase<TCommand, TResponse>
    {
        public abstract TResponse ExecuteCommand(TCommand command);
    }
}
