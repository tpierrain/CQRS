using System;

namespace BookARoom.Domain.ReadModel
{
    public class Reservation
    {
        public Guid Guid { get; private set; }
        public string ClientId { get; }
        public string HotelName { get; set; }
        public string HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }


        public Reservation(Guid guid, string clientId, string hotelName, string hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.Guid = guid;
            this.ClientId = clientId;
            this.HotelName = hotelName;
            this.HotelId = hotelId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }

        public override string ToString()
        {
            return $"Reservation for:{ClientId} at Hotel:{HotelName} (id:{HotelId}), RoomNumber:{RoomNumber} for check-in date:{CheckInDate.ToString("d")} and check-out date:{CheckOutDate.ToString("d")}";
        }
    }
}