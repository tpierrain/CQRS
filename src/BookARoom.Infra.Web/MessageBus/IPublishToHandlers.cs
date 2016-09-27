using System;
using BookARoom.Domain;

namespace BookARoom.Infra.MessageBus
{
    public interface IPublishToHandlers
    {
        void PublishTo<T>(Action<Message> handler, T @event) where T : Event;
    }
}