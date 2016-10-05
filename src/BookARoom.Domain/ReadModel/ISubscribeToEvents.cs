using System;

namespace BookARoom.Domain.ReadModel
{
    public interface ISubscribeToEvents
    {
        void RegisterHandler<T>(Action<T> handler) where T : IMessage;
    }

    // to sustain CQRS explanations at MS experiences'16
    public interface ISubscribeToCommands : ISubscribeToEvents
    {
    }
}