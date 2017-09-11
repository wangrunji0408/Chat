namespace Chat.Server.Domains.Events.Chatroom
{
    class NewChatroomEvent: ChatroomEvent
    {
        public long CreatorId { get; }
        
        public NewChatroomEvent(long chatroomId, long creatorId)
        {
            ChatroomId = chatroomId;
            CreatorId = creatorId;
        }
    }
}