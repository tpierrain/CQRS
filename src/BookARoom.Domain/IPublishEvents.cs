namespace BookARoom.Domain
{
    public interface IPublishEvents
    {
        void PublishTo<T>(T @event) where T : IEvent;
    }
}