namespace Process.Pipeline
{
    using MediatR;

    public abstract class Query<TResponse> : IRequest<TResponse>
    {
    }
}
