using System;

namespace BookARoom.Domain.WriteModel
{
    public class RoomBooked : IEvent
    {
        public int PlaceId { get; }
        public string ClientId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public RoomBooked(int placeId, string clientId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            PlaceId = placeId;
            ClientId = clientId;
            RoomNumber = roomNumber;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
        }
    }
}