namespace Process.Features.Audio
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class Index
    {
        public class Query : Pipeline.Query<IEnumerable<Model>>
        {
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            public Task<IEnumerable<Model>> Handle(
                Query request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(Enumerable.Empty<Model>());
            }
        }

        public class Model
        {
            public string Title { get; set; }
            public long Duration { get; set; }
        }
    }
}