using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Provides Hotels to stay.
    /// <remarks>Repository of Hotels.</remarks>
    /// </summary>
    public interface IProvideHotel
    {
        IEnumerable<Hotel> SearchFromLocation(string location);
        Hotel GetHotel(int hotelId);
    }
}