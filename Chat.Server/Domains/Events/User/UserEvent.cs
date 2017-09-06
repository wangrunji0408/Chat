namespace Chat.Server.Domains.Events.User
{
    abstract class UserEvent: DomainEvent
    {
        public long UserId { get; protected set; }
    }
}