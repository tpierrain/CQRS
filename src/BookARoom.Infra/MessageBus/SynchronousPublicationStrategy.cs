using System;
using BookARoom.Domain;

namespace BookARoom.Infra.MessageBus
{
    public class SynchronousPublicationStrategy : IPublishToHandlers
    {
        public void PublishTo<T>(Action<IMessage> handler, T @event) where T : IEvent
        {
            handler(@event); // synchronous publication to simplify the test of this first step
        }
    }
}