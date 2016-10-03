using System;

namespace BookARoom.Domain.WriteModel
{
    public class Booking
    {
        // We provide getters only so that the state of this domain object is only changed via one of its operations (methods)
        public Guid BookingId { get; }
        public string ClientId { get; }
        public int HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public Booking(Guid bookingId , string clientId, int hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.BookingId = bookingId;
            this.ClientId = clientId;
            this.HotelId = hotelId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }
}