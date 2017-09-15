namespace Chat.Server.Domains.Events.User
{
    abstract class UserEvent: DomainEvent
    {
        public long UserId { get; set; }
    }
    class BecameFriendsEvent : UserEvent
    {
        public long User2Id { get; set; }
    }
    class UserLoginEvent : UserEvent
    {
    }
    class UserSignupEvent : UserEvent
    { 
    }
}