namespace Chat.Server.Domains.Events.User
{
    class BecameFriendsEvent: UserEvent
    {
        public long User2Id { get; set; }
        
        public BecameFriendsEvent(long user1Id, long user2Id)
        {
            UserId = user1Id;
            User2Id = user2Id;
        }

        public override string ToString()
        {
            return $"[{nameof(BecameFriendsEvent)} " +
                   $"{nameof(UserId)}: {UserId}, " + 
                   $"{nameof(User2Id)}: {User2Id}]";
        }
    }
}