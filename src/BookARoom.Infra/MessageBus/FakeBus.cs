using System;
using System.Collections.Generic;
using BookARoom.Domain;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.MessageBus
{
    /// <summary>
    /// Class coming from Greg YOUNG's https://github.com/gregoryyoung/m-r repo (thanks Greg!).
    /// (I just added the <see cref="ISubscribeToEvents"/> interface for my own needs and 
    /// slightly changed the Publish method to introduce a synchronous/asynchronous strategy).
    /// </summary>
    public class FakeBus : ISendCommands, IPublishEvents, ISubscribeToEvents
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();
        private readonly IPublishToHandlers publicationStrategy;

        public FakeBus(bool synchronousPublication = true)
        {
            if (synchronousPublication)
            {
                this.publicationStrategy = new SynchronousPublicationStrategy();
            }
            else
            {
                this.publicationStrategy = new AsynchronousThreadPoolPublicationStrategy();
            }
        }

        public void RegisterHandler<T>(Action<T> handler) where T : IMessage
        {
            List<Action<IMessage>> handlers;

            if(!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }

            handlers.Add((x => handler((T)x)));
        }

        public void Send<T>(T command) where T : ICommand
        {
            List<Action<IMessage>> handlers;

            if (_routes.TryGetValue(typeof(T), out handlers))
            {
                if (handlers.Count != 1) throw new InvalidOperationException("cannot send to more than one handler");
                handlers[0](command);
            }
            else
            {
                throw new InvalidOperationException("no handler registered");
            }
        }

        public void PublishTo<T>(T @event) where T : IEvent
        {
            List<Action<IMessage>> handlers;

            if (!_routes.TryGetValue(@event.GetType(), out handlers)) return;

            foreach(var handler in handlers)
            {
                this.publicationStrategy.PublishTo(handler, @event);
            }
        }
    }
}
