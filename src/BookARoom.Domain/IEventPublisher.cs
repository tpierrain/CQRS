namespace BookARoom.Domain
{
    public interface IEventPublisher
    {
        void PublishTo<T>(T @event) where T : IEvent;
    }
}