using System;
using BookARoom.Domain;

namespace BookARoom.Infra.MessageBus
{
    public interface IPublishToHandlers
    {
        void PublishTo<T>(Action<IMessage> handler, T @event) where T : IEvent;
    }
}