using System;
using System.Collections.Generic;

namespace BookARoom.Domain
{
    public class RoomSearchEngine
    {
        private readonly ICatalogPlaces places;

        public RoomSearchEngine(ICatalogPlaces places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchPlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int roomNumber = 1, int childrenCount = 0)
        {
            if (checkInDate > checkOutDate)
            {
                throw new InvalidOperationException($"Check out date ({checkOutDate}) must be after Check in date ({checkInDate}).");
            }

            return this.places.SearchPlacesInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }
    }
}