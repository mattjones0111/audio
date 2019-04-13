namespace Process.Features.Audio
{
    using System.Collections.Generic;
    using MediatR;

    public class Index
    {
        public class Query : IRequest<IEnumerable<Model>>
        {
        }

        public class Model
        {
        }
    }
}