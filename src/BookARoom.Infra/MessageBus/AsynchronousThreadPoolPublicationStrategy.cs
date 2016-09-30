using System;
using System.Threading;
using BookARoom.Domain;

namespace BookARoom.Infra.MessageBus
{
    public class AsynchronousThreadPoolPublicationStrategy : IPublishToHandlers
    {
        public void PublishTo<T>(Action<IMessage> handler, T @event) where T : IEvent
        {
            //dispatch on thread pool for added awesomeness
            ThreadPool.QueueUserWorkItem(x => handler(@event));
        }
    }
}