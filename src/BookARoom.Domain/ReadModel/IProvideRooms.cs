using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Find rooms.
    /// <remarks>Repository of Rooms.</remarks>
    /// </summary>
    public interface IProvideRooms
    {
        // TODO: return IEnumerable<Rooms> instead
        IEnumerable<Place> SearchAvailablePlacesInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate);
    }
}