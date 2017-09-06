namespace Chat.Server.Domains.Events.Chatroom
{
    class UserEnterChatroomEvent: ChatroomEvent
    {
        public long UserId { get; protected set; }

        public UserEnterChatroomEvent(long chatroomId, long userId)
        {
            UserId = userId;
            ChatroomId = chatroomId;
        }

        public override string ToString()
        {
            return $"[{nameof(UserEnterChatroomEvent)} " +
                   $"{nameof(UserId)}: {UserId}, " +
                   $"{nameof(ChatroomId)}: {ChatroomId}]";
        }
    }
}