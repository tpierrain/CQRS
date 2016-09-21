namespace BookARoom.Domain.WriteModel
{
    public interface IClientAndBookingRepository
    {
        void Save(BookARoomCommand command);
        bool IsClientAlready(string clientIdentifier);
        void CreateClient(string clientIdentifier);
        long BookingCount { get; }
    }
}