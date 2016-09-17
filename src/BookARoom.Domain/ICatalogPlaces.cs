using System.Collections.Generic;

namespace BookARoom.Domain
{
    public interface ICatalogPlaces
    {
        IEnumerable<Place> SearchFromLocation(string location);
    }
}