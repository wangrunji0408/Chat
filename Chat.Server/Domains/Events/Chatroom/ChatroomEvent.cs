using System;

namespace Chat.Server.Domains.Events
{
    abstract class ChatroomEvent: IDomainEvent
    {
        public long ChatroomId { get; protected set; }
        public DateTimeOffset Time { get; protected set; } = DateTimeOffset.Now;
    }
}