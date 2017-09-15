using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Chat.Server.Domains.Events;

namespace Chat.Server.Infrastructure.EventBus
{
    public class EventBus: IEventBus
    {
        private readonly Subject<DomainEvent> subject = new Subject<DomainEvent>();

        public IObservable<T> GetEventStream<T>() where T : DomainEvent
        {
            return subject.OfType<T>();
        }

        public void Publish<T>(T @event) where T : DomainEvent
        {
            subject.OnNext(@event);
        }
    }
}