using System;

namespace BookARoom.Domain.ReadModel
{
    public interface ISubscribeToEvents
    {
        void RegisterHandler<T>(Action<T> handler) where T : IMessage;
    }
}