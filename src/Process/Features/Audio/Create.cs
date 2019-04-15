﻿namespace Process.Features.Audio
{
    using System;
    using System.Threading.Tasks;
    using Domain.AudioItem;
    using FluentValidation;
    using Pipeline;
    using Ports;

    public class Create
    {
        public class Command : Pipeline.Command
        {
            public string Title { get; set; }
            public TimeSpan Duration { get; set; }
            public string[] Categories { get; set; }
            public string Source { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Title)
                    .NotEmpty();

                RuleFor(x => x.Duration)
                    .Must(BeNonNegativeDuration);

                RuleFor(x => x.Source)
                    .Must(BeValidUrl);
            }

            bool BeValidUrl(string arg)
            {
                return Uri.TryCreate(arg, UriKind.Absolute, out Uri _);
            }

            bool BeNonNegativeDuration(TimeSpan arg)
            {
                return arg >= TimeSpan.Zero;
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
                    command.Title,
                    command.Duration,
                    command.Categories);

                await documentStore.StoreAsync(item.ToDocument());

                return CommandResult.Void;
            }
        }
    }
}