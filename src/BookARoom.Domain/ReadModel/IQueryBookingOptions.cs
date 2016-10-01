using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Interface to interact with our system in order to query booking options.
    /// </summary>
    public interface IQueryBookingOptions
    {
        IEnumerable<BookingOption> SearchBookingOptions(SearchBookingOptions query);
    }
}