using System;

namespace BookARoom.Infra.Web.ViewModels
{
    public class SearchRoomQueryViewModel
    {
        public string Destination { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumberOfAdults { get; set; }

        public SearchRoomQueryViewModel()
        {
        }

        public SearchRoomQueryViewModel(string destination, DateTime checkInDate, DateTime checkOutDate, int numberOfAdults)
        {
            this.Destination = destination;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
            this.NumberOfAdults = numberOfAdults;
        }
    }
}