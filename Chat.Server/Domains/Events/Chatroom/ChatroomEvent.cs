using System;

namespace Chat.Server.Domains.Events.Chatroom
{
    abstract class ChatroomEvent: DomainEvent
    {
        public long ChatroomId { get; protected set; }
    }
}