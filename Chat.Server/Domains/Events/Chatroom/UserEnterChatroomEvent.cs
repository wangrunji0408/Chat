namespace Chat.Server.Domains.Events
{
    class UserEnterChatroomEvent: ChatroomEvent
    {
        public long UserId { get; protected set; }

        public UserEnterChatroomEvent(long chatroomId, long userId)
        {
            UserId = userId;
            ChatroomId = chatroomId;
        }
    }
}