namespace BookARoom.Domain.WriteModel
{
    public class BookingStore : IBookRooms
    {
        private readonly ISaveBookingCommandsAndClients repository;
        private readonly IEventPublisher eventPublisher;

        public BookingStore(ISaveBookingCommandsAndClients repository, IEventPublisher eventPublisher)
        {
            this.repository = repository;
            this.eventPublisher = eventPublisher;
        }

        public void BookARoom(BookARoomCommand bookingCommand)
        {
            if (!this.repository.IsClientAlready(bookingCommand.ClientId))
            {
                this.repository.CreateClient(bookingCommand.ClientId);    
            }

            this.repository.Save(bookingCommand);

            var roomBooked = new RoomBooked(bookingCommand.PlaceId, bookingCommand.ClientId, bookingCommand.RoomNumber, bookingCommand.CheckInDate, bookingCommand.CheckOutDate);
            this.eventPublisher.PublishTo(roomBooked);
        }
    }
}
