namespace Process.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public abstract class CommandHandler<TCommand> :
        IRequestHandler<TCommand, CommandResult>
        where TCommand : IRequest<CommandResult>
    {
        protected abstract Task<CommandResult> HandleImpl(TCommand command);

        public Task<CommandResult> Handle(
            TCommand request,
            CancellationToken cancellationToken)
        {
            return HandleImpl(request);
        }
    }
}