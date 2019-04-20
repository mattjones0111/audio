namespace Process.Features.Audio.Categories
{
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Aspects.Audit;
    using Aspects.Validation;
    using Domain.AudioItem;
    using FluentValidation;
    using Pipeline;
    using Ports;

    public class Remove
    {
        [AuditDescription("Remove an audio item from a category")]
        public class Command : Pipeline.Command
        {
            public string Id { get; set; }
            public string Category { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            readonly IStoreDocuments documentStore;

            public Validator(IStoreDocuments documentStore)
            {
                this.documentStore = documentStore;

                RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithHttpStatusCode(HttpStatusCode.BadRequest)
                    .MustAsync(Exist)
                    .WithHttpStatusCode(HttpStatusCode.NotFound);

                RuleFor(x => x.Category)
                    .NotEmpty();
            }

            Task<bool> Exist(string id, CancellationToken cancellationToken)
            {
                return documentStore.ExistsAsync<State>(id);
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

                aggregate.Categories.Remove(command.Category);

                await documentStore.StoreAsync(aggregate.ToDocument());

                return CommandResult.Void;
            }
        }
    }
}