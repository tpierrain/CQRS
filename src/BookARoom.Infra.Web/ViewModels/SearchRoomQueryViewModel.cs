using System;

namespace BookARoom.Infra.Web.ViewModels
{
    public class SearchRoomQueryViewModel
    {
        public string Destination { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}