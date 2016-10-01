using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Find rooms.
    /// <remarks>Repository of AvailableRoomsWithPrices.</remarks>
    /// </summary>
    public interface IProvideRooms
    {
        // TODO: return IEnumerable<AvailableRoomsWithPrices> instead
        IEnumerable<BookingOption> SearchAvailableHotelsInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate);
    }
}