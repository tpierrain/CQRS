using System;

namespace BookARoom.Domain.WriteModel
{
    public class BookARoomCommand : ICommand
    {
        public string ClientId { get; }
        public int PlaceId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public BookARoomCommand(string clientId, int placeId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.ClientId = clientId;
            this.PlaceId = placeId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }
}