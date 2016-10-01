using System;

namespace BookARoom.Domain.ReadModel
{
    public class SearchBookingProposal : Query
    {
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public string Location { get; }
        public int NumberOfAdults { get; }
        public int NumberOfRoomsNeeded { get; }
        public int ChildrenCount { get; }

        public SearchBookingProposal(DateTime checkInDate, DateTime checkOutDate, string location, int numberOfAdults, int numberOfRoomsNeeded = 1, int childrenCount = 0)
        {
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
            this.Location = location;
            this.NumberOfAdults = numberOfAdults;
            this.NumberOfRoomsNeeded = numberOfRoomsNeeded;
            this.ChildrenCount = childrenCount;
        }
    }
}