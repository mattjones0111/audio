namespace Process.Features.Audio.Categories
{
    using System.Threading.Tasks;
    using Domain.AudioItem;
    using FluentValidation;
    using Pipeline;
    using Ports;

    public class Add
    {
        public class Command : Pipeline.Command
        {
            public string Id { get; set; }
            public string Category { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.Category)
                    .NotEmpty();
            }
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

                aggregate.Categories.Add(command.Category);

                await documentStore.StoreAsync(aggregate.ToDocument());

                return CommandResult.Void;
            }
        }
    }
}
