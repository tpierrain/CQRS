using System;

namespace BookARoom.Domain.WriteModel
{
    public class BookingStore : IBookRooms
    {
        private readonly ISaveBooking saveBooking;
        private readonly IHandleClients handleClients;
        private readonly IPublishEvents publishEvents;

        public BookingStore(ISaveBooking saveBooking, IHandleClients handleClients,  IPublishEvents publishEvents)
        {
            this.saveBooking = saveBooking;
            this.handleClients = handleClients;
            this.publishEvents = publishEvents;
        }

        public void BookARoom(BookingCommand command)
        {
            if (!this.handleClients.IsClientAlready(command.ClientId))
            {
                this.handleClients.CreateClient(command.ClientId);    
            }

            Guid guid = Guid.NewGuid();
            var booking = new Booking(guid, command.ClientId, command.HotelId, command.RoomNumber, command.CheckInDate, command.CheckOutDate);
            this.saveBooking.Save(booking);

            // we could enrich the event from here (eg. finding the HotelName from the HotelId)
            var roomBooked = new RoomBooked(guid, command.HotelName, command.HotelId, command.ClientId, command.RoomNumber, command.CheckInDate, command.CheckOutDate);
            this.publishEvents.PublishTo(roomBooked);
        }
    }
}
