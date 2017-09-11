namespace Chat.Server.Domains.Events.Chatroom
{
    class DismissedEvent: ChatroomEvent
    {
        public long OperatorId { get; set; }

        public DismissedEvent(long roomId, long operatorId)
        {
            ChatroomId = roomId;
            OperatorId = operatorId;
        }
    }
}