using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    /// <summary>
    /// Provides Places to stay.
    /// <remarks>Repository of Places.</remarks>
    /// </summary>
    public interface IProvidePlaces
    {
        IEnumerable<Place> SearchFromLocation(string location);
        Place GetPlace(int placeId);
    }
}