using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Interface to interact with our system.
    /// </summary>
    public interface ISearchBookingProposals
    {
        IEnumerable<BookingProposal> SearchBookingProposals(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int roomNumber = 1, int childrenCount = 0);
    }
}