namespace Process.Features.Health
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class Index
    {
        public class Query : Pipeline.Query<Model>
        {
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            public Task<Model> Handle(
                Query request,
                CancellationToken cancellationToken)
            {
                return Task.FromResult(new Model()
                {
                    ServerTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                });
            }
        }

        public class Model
        {
            public long ServerTime { get; set; }
        }
    }
}
