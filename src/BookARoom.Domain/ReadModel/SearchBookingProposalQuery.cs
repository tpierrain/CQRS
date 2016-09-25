using System;

namespace BookARoom.Domain.ReadModel
{
    public class SearchBookingProposalQuery
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Location { get; set; }
        public int AdultsCount { get; set; }
        public int NumberOfRoomsNeeded { get; set; }
        public int ChildrenCount { get; set; }

        public SearchBookingProposalQuery(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int numberOfRoomsNeeded = 1, int childrenCount = 0)
        {
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            Location = location;
            AdultsCount = adultsCount;
            NumberOfRoomsNeeded = numberOfRoomsNeeded;
            ChildrenCount = childrenCount;
        }
    }
}