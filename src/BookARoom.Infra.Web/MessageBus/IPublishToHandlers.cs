using System;
using BookARoom.Domain;

namespace BookARoom.Infra.Web.MessageBus
{
    public interface IPublishToHandlers
    {
        void PublishTo<T>(Action<IMessage> handler, T @event) where T : IEvent;
    }
}