namespace Chat.Server.Domains.Events.Chatroom
{
    class NewChatroomEvent: ChatroomEvent
    {
        public NewChatroomEvent(long chatroomId)
        {
            ChatroomId = chatroomId;
        }
    }
}