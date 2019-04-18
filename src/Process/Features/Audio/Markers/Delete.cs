namespace Process.Features.Audio.Markers
{
    using System.Threading.Tasks;
    using Pipeline;

    public class Delete
    {
        public class Command : Pipeline.Command
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public long Offset { get; set; }
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