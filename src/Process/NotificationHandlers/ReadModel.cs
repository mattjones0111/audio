namespace Process.NotificationHandlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Features.Audio;
    using MediatR;
    using Ports;

    public class ReadModel : INotificationHandler<Create.AudioItemCreated>
    {
        readonly IStoreTabularData tableStore;

        public ReadModel(IStoreTabularData tableStore)
        {
            this.tableStore = tableStore;
        }

        public Task Handle(
            Create.AudioItemCreated notification,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
