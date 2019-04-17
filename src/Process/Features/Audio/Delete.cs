namespace Process.Features.Audio
{
    using System.Threading.Tasks;
    using Pipeline;

    public class Delete
    {
        public class Command : Pipeline.Command
        {
        }

        public class Handler : CommandHandler<Command>
        {
            protected override Task<CommandResult> HandleImpl(Command command)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}