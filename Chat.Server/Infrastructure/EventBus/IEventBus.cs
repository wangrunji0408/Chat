using System;
using Chat.Server.Domains.Events;

namespace Chat.Server.Infrastructure.EventBus
{
    public interface IEventBus
    {
        IObservable<T> GetEventStream<T>() where T : DomainEvent;
        void Publish<T>(T @event) where T : DomainEvent;
    }
}