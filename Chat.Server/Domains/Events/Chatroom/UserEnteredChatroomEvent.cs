namespace Chat.Server.Domains.Events.Chatroom
{
    class UserEnteredChatroomEvent: ChatroomEvent
    {
        public long UserId { get; protected set; }

        public UserEnteredChatroomEvent(long chatroomId, long userId)
        {
            UserId = userId;
            ChatroomId = chatroomId;
        }

        public override string ToString()
        {
            return $"[{nameof(UserEnteredChatroomEvent)} " +
                   $"{nameof(UserId)}: {UserId}, " +
                   $"{nameof(ChatroomId)}: {ChatroomId}]";
        }
    }
}