namespace BookARoom.Domain.WriteModel
{
    public class BookingStore : IBookRooms
    {
        private readonly ISaveBookingCommandsAndClients repository;

        public BookingStore(ISaveBookingCommandsAndClients repository)
        {
            this.repository = repository;
        }

        public void BookARoom(BookARoomCommand bookingCommand)
        {
            if (!this.repository.IsClientAlready(bookingCommand.ClientId))
            {
                this.repository.CreateClient(bookingCommand.ClientId);    
            }

            this.repository.Save(bookingCommand);
        }
    }
}
