namespace Chat.Server.Domains.Events.User
{
    class UserLoginEvent: UserEvent
    {
        public UserLoginEvent(long userId)
        {
            UserId = userId;
        }

        public override string ToString()
        {
            return $"[{nameof(UserLoginEvent)} " +
                   $"{nameof(UserId)} {UserId}]";
        }
    }
}