using System;

namespace BookARoom.Domain.ReadModel
{
    public class Reservation
    {
        public string ClientId { get; }
        public string HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public Reservation(string clientId, string hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.ClientId = clientId;
            this.HotelId = hotelId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }

        public override string ToString()
        {
            return $"Reservation for:{ClientId} at HotelID:{HotelId}, RoomNumber:{RoomNumber} for check-in date:{CheckInDate.ToString("d")} and check-out date:{CheckOutDate.ToString("d")}";
        }
    }
}