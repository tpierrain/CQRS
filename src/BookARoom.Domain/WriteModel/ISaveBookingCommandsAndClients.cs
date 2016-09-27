namespace BookARoom.Domain.WriteModel
{
    public interface ISaveBookingCommandsAndClients
    {
        void Save(BookARoomCommand bookingCommand);
        bool IsClientAlready(string clientIdentifier);
        void CreateClient(string clientIdentifier);
    }
}