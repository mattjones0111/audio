namespace Process.Features.Audio.Markers
{
    using System.Threading.Tasks;
    using Domain.AudioItem;
    using Pipeline;
    using Ports;

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
            readonly IStoreDocuments documentStore;

            public Handler(IStoreDocuments documentStore)
            {
                this.documentStore = documentStore;
            }

            protected override async Task<CommandResult> HandleImpl(Command command)
            {
                State state = await documentStore.GetAsync<State>(command.Id);

                Aggregate aggregate = new Aggregate(state);

                aggregate.Markers.Delete(command.Name, command.Offset);

                await documentStore.StoreAsync(aggregate.ToDocument());

                return CommandResult.Void;
            }
        }
    }
}
