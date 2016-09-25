using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Interface to interact with our system in order to query booking proposals.
    /// </summary>
    public interface IQueryBookingProposals
    {
        IEnumerable<BookingProposal> SearchBookingProposals(SearchBookingProposalQuery query);
    }
}