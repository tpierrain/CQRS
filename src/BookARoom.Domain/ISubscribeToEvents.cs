using System;

namespace BookARoom.Domain
{
    public interface ISubscribeToEvents
    {
        void RegisterHandler<T>(Action<T> handler) where T : Message;
    }
}