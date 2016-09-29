using System;

namespace BookARoom.Domain.ReadModel
{
    public class SearchBookingProposal : Query
    {
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }
        public string Location { get; }
        public int AdultsCount { get; }
        public int NumberOfRoomsNeeded { get; }
        public int ChildrenCount { get; }

        public SearchBookingProposal(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int numberOfRoomsNeeded = 1, int childrenCount = 0)
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