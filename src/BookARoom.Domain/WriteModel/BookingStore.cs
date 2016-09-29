namespace BookARoom.Domain.WriteModel
{
    public class BookingStore : IBookRooms
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IClientRepository clientRepository;
        private readonly IPublishEvents publishEvents;

        public BookingStore(IBookingRepository bookingRepository, IClientRepository clientRepository,  IPublishEvents publishEvents)
        {
            this.bookingRepository = bookingRepository;
            this.clientRepository = clientRepository;
            this.publishEvents = publishEvents;
        }

        public void BookARoom(BookARoomCommand bookingCommand)
        {
            if (!this.clientRepository.IsClientAlready(bookingCommand.ClientId))
            {
                this.clientRepository.CreateClient(bookingCommand.ClientId);    
            }

            this.bookingRepository.Save(bookingCommand);

            var roomBooked = new RoomBooked(bookingCommand.PlaceId, bookingCommand.ClientId, bookingCommand.RoomNumber, bookingCommand.CheckInDate, bookingCommand.CheckOutDate);
            this.publishEvents.PublishTo(roomBooked);
        }
    }
}
