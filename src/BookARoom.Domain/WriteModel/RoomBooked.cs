using System;

namespace BookARoom.Domain.WriteModel
{
    public class RoomBooked : IEvent
    {
        public int HotelId { get; }
        public string ClientId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public RoomBooked(int hotelId, string clientId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.HotelId = hotelId;
            this.ClientId = clientId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }
}