namespace BookARoom.Domain
{
    public interface IEvent : IMessage
    {
        // public int Version; // no time for Event Sourcing here.
    }
}