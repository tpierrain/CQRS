using System;

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

        public void BookARoom(BookingCommand command)
        {
            if (!this.clientRepository.IsClientAlready(command.ClientId))
            {
                this.clientRepository.CreateClient(command.ClientId);    
            }

            Guid guid = Guid.NewGuid();
            var booking = new Booking(guid, command.ClientId, command.HotelId, command.RoomNumber, command.CheckInDate, command.CheckOutDate);
            this.bookingRepository.Save(booking);

            // we could enrich the event from here (eg. finding the HotelName from the HotelId)
            var roomBooked = new RoomBooked(guid, command.HotelName, command.HotelId, command.ClientId, command.RoomNumber, command.CheckInDate, command.CheckOutDate);
            this.publishEvents.PublishTo(roomBooked);
        }
    }
}
