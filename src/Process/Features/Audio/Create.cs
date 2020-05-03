namespace Process.Features.Audio
{
    using System.Threading.Tasks;
    using Aspects.Audit;
    using Domain.AudioItem;
    using FluentValidation;
    using MediatR;
    using Pipeline;
    using Ports;

    public class Create
    {
        [AuditDescription("Create an audio item")]
        public class Command : Pipeline.Command
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string[] Categories { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.Title)
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

            protected override async Task<CommandResult> HandleImpl(
                Command command)
            {
                Aggregate item = new Aggregate(
                    command.Id,
                    command.Title,
                    command.Categories);

                await documentStore.StoreAsync(item.ToDocument());

                return CommandResult.Void
                    .WithNotification(new AudioItemCreated
                    {
                        Title = command.Title
                    });
            }
        }

        public class AudioItemCreated : INotification
        {
            public string Title { get; set; }
        }
    }
}
