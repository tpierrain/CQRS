using System.Collections.Generic;

namespace BookARoom
{
    public interface ICatalogPlaces
    {
        IEnumerable<Place> SearchFromLocation(string location);
    }
}