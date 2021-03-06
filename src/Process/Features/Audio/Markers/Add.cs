﻿namespace Process.Features.Audio.Markers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Aspects.Audit;
    using Aspects.Validation;
    using Domain.AudioItem;
    using FluentValidation;
    using Pipeline;
    using Ports;

    public class Add
    {
        [AuditDescription("Add a marker to an audio item")]
        public class Command : Pipeline.Command
        {
            public string Id { get; set; }
            public long Offset { get; set; }
            public string Name { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            readonly IStoreDocuments docStore;

            public Validator(IStoreDocuments docStore)
            {
                this.docStore = docStore;

                RuleFor(x => x.Id)
                    .MustAsync(Exist)
                    .WithHttpStatusCode(HttpStatusCode.NotFound);

                RuleFor(x => x.Offset)
                    .MustAsync(NotAlreadyExist)
                    .WithHttpStatusCode(HttpStatusCode.Conflict);
            }

            async Task<bool> NotAlreadyExist(
                Command command,
                long offset,
                CancellationToken cancellationToken)
            {
                State state = await docStore.GetAsync<State>(command.Id);

                return !state.Markers.Any(x =>
                    string.Equals(x.Name, command.Name, StringComparison.InvariantCultureIgnoreCase) &&
                    x.Offset == offset);
            }

            Task<bool> Exist(string id, CancellationToken cancellationToken)
            {
                return docStore.ExistsAsync<State>(id);
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
                State state = await documentStore
                    .GetAsync<State>(command.Id);

                Aggregate aggregate = new Aggregate(state);

                aggregate.Markers.Add(command.Name, command.Offset);

                await documentStore.StoreAsync(aggregate.ToDocument());

                return CommandResult.Void;
            }
        }
    }
}
