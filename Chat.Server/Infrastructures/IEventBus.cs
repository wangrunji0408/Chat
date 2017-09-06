using System;
using Chat.Server.Domains.Events;

namespace Chat.Server.Infrastructures
{
    public interface IEventBus
    {
        IObservable<T> GetEventStream<T>() where T : IDomainEvent;
        void Publish<T>(T @event) where T : IDomainEvent;
    }
}