using System;

namespace BookARoom.Domain.WriteModel
{
    public class RoomBooked : IEvent
    {
        public string HotelName { get; }
        public int HotelId { get; }
        public string ClientId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public RoomBooked(string hotelName,int hotelId, string clientId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.HotelName = hotelName;
            this.HotelId = hotelId;
            this.ClientId = clientId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }
}