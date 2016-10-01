using System;

namespace BookARoom.Infra.Web.ViewModels
{
    public class BookingRequestViewModel
    {
        public string ClientMail { get; set; }
        public string HotelName { get; set; }
        public string HotelId { get; set; }
        public string RoomId { get; set; }

        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public bool BookingSucceeded { get; set; }
    }
}