using System;

namespace Chat.Server.Domains.Events
{
    public abstract class DomainEvent
    {
        public DateTimeOffset Time { get; protected set; } = DateTimeOffset.Now;
    }
}