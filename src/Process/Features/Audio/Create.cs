namespace Process.Features.Audio
{
    using System;
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
            public string Source { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty();

                RuleFor(x => x.Title)
                    .NotEmpty();

                RuleFor(x => x.Source)
                    .Must(BeValidUrl);
            }

            bool BeValidUrl(string arg)
            {
                return Uri.TryCreate(arg, UriKind.Absolute, out Uri _);
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
                TimeSpan timespan = TimeSpan.FromMinutes(3.5);

                Aggregate item = new Aggregate(
                    command.Id,
                    command.Title,
                    timespan,
                    command.Categories);

                await documentStore.StoreAsync(item.ToDocument());

                return CommandResult.Void
                    .WithNotification(new AudioItemCreated
                    {
                        Title = command.Title,
                        Duration = (long)timespan.TotalMilliseconds
                    });
            }
        }

        public class AudioItemCreated : INotification
        {
            public string Title { get; set; }
            public long Duration { get; set; }
        }
    }
}
