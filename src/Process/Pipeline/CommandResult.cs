namespace Process.Pipeline
{
    using System.Collections.Generic;
    using MediatR;

    public class CommandResult
    {
        public static CommandResult Void => new CommandResult();

        CommandResult()
        {
        }

        readonly List<INotification> notifications = new List<INotification>();

        public CommandResult WithNotification(INotification notification)
        {
            notifications.Add(notification);
            return this;
        }

        public IReadOnlyCollection<INotification> GetNotifications()
        {
            return notifications.AsReadOnly();
        }

        object responseObject;

        public CommandResult WithResponse(object response)
        {
            responseObject = response;
            return this;
        }

        public bool HasResponse => responseObject != null;

        public object Response => responseObject;
    }
}
