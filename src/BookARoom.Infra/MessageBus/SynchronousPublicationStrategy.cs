using System;
using BookARoom.Domain;

namespace BookARoom.Infra.MessageBus
{
    public class SynchronousPublicationStrategy : IPublishToHandlers
    {
        public void PublishTo<T>(Action<Message> handler, T @event) where T : Event
        {
            handler(@event); // synchronous publication to simplify the test of this first step
        }
    }
}