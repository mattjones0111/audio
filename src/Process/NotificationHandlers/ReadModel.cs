namespace Process.NotificationHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Features.Audio;
    using MediatR;

    public class ReadModel : INotificationHandler<Create.AudioItemCreated>
    {
        public Task Handle(
            Create.AudioItemCreated notification,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
