namespace Process.Pipeline
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class FeatureBehaviour<TRequest, TResponse> :
        IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            return await next();
        }
    }
}
