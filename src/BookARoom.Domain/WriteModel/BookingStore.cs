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

            // we could enrich the event from here (eg. finding the HotelName from the HotelId)
            var roomBooked = new RoomBooked(bookingCommand.HotelName, bookingCommand.HotelId, bookingCommand.ClientId, bookingCommand.RoomNumber, bookingCommand.CheckInDate, bookingCommand.CheckOutDate);
            this.publishEvents.PublishTo(roomBooked);
        }
    }
}
