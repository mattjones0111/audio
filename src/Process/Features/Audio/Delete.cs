namespace Process.Features.Audio
{
    using System;
    using System.Threading.Tasks;
    using Domain.AudioItem;
    using Pipeline;
    using Ports;

    public class Delete
    {
        public class Command : Pipeline.Command
        {
            public Guid Id { get; set; }
        }

        public class Handler : CommandHandler<Command>
        {
            readonly IStoreDocuments documentStore;

            public Handler(IStoreDocuments documentStore)
            {
                this.documentStore = documentStore;
            }

            protected override async Task<CommandResult> HandleImpl(
                Command command)
            {
                await documentStore.DeleteAsync<State>(command.Id.ToString());

                return CommandResult.Void;
            }
        }
    }
}
