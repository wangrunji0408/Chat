namespace Chat.Server.Events
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