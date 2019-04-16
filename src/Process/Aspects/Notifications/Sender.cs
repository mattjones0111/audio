namespace Process.Aspects.Notifications
{
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using MediatR.Pipeline;
    using Pipeline;

    public class Sender<TRequest, TResponse> :
        IRequestPostProcessor<TRequest, TResponse>
    {
        readonly IMediator mediator;

        public Sender(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task Process(TRequest request, TResponse response)
        {
            if(response is CommandResult commandResult)
            {
                await Task.WhenAll(commandResult
                    .GetNotifications()
                    .Select(x => mediator.Publish(x)));
            }
        }
    }
}