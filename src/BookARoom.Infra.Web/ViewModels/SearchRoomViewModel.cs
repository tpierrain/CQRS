using System;

namespace BookARoom.Infra.Web.ViewModels
{
    public class SearchRoomViewModel
    {
        public string Destination { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}