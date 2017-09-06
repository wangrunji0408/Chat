using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Chat.Server.Domains.Events;

namespace Chat.Server.Infrastructures
{
    public class EventBus: IEventBus
    {
        private readonly Subject<IDomainEvent> subject = new Subject<IDomainEvent>();

        public IObservable<T> GetEventStream<T>() where T : IDomainEvent
        {
            return subject.OfType<T>();
        }

        public void Publish<T>(T @event) where T : IDomainEvent
        {
            subject.OnNext(@event);
        }
    }
}