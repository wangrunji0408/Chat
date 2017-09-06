namespace Chat.Server.Domains.Events.User
{
    class UserSignupEvent: UserEvent
    {
        public UserSignupEvent(long userId)
        {
            UserId = userId;
        }

        public override string ToString()
        {
            return $"[{nameof(UserSignupEvent)} " +
                   $"{nameof(UserId)} {UserId}]";
        }
    }
}