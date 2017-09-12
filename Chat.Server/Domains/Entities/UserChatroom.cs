namespace Chat.Server.Domains.Entities
{
    class UserChatroom
    {
        public long UserId { get; set; }
        public User User { get; set; }

        public long ChatroomId { get; set; }
        public Chatroom Chatroom { get; set; }

        public string NameInChatroom { get; set; }

        public enum UserRole
        {
            Normal, Admin
        }

        public UserRole Role { get; set; } = UserRole.Normal;
        public bool IsBlocked { get; set; }

        public override string ToString()
        {
            return $"[UserChatroom {nameof(UserId)}: {UserId}, " +
                   $"{nameof(ChatroomId)}: {ChatroomId}, " +
                   $"{nameof(NameInChatroom)}: {NameInChatroom}, " +
                   $"{nameof(Role)}: {Role}, " +
                   $"{nameof(IsBlocked)}: {IsBlocked}]";
        }
    }
}
