using System;
using System.Collections.Generic;

namespace BookARoom.Domain
{
    /// <summary>
    /// Search available places to stay.
    /// </summary>
    public class RoomSearchEngine
    {
        private readonly ICatalogPlaces places;

        public RoomSearchEngine(ICatalogPlaces places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchAvailablePlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int roomNumber = 1, int childrenCount = 0)
        {
            if (checkInDate > checkOutDate)
            {
                throw new InvalidOperationException($"Check out date ({checkOutDate}) must be after Check in date ({checkInDate}).");
            }

            return this.places.SearchAvailablePlacesInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }
    }
}