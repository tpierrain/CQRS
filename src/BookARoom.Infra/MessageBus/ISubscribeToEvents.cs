using System;
using BookARoom.Domain;

namespace BookARoom.Infra.MessageBus
{
    public interface ISubscribeToEvents
    {
        void RegisterHandler<T>(Action<T> handler) where T : IMessage;
    }
}