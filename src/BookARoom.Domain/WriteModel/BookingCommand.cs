using System;

namespace BookARoom.Domain.WriteModel
{
    public class BookingCommand : ICommand
    {
        public string ClientId { get; }
        public string HotelName { get; }
        public int HotelId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public BookingCommand(string clientId, string hotelName, int hotelId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.ClientId = clientId;
            this.HotelName = hotelName;
            this.HotelId = hotelId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }
}