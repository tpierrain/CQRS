using System;
using System.Collections.Generic;

namespace BookARoom.Domain
{
    public class RoomBookingEngine
    {
        private readonly ICatalogPlaces places;

        public RoomBookingEngine(ICatalogPlaces places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchPlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int adultsCount, int roomNumber = 1, int childrenCount = 0)
        {
            if (checkInDate > checkOutDate)
            {
                throw new InvalidOperationException($"Check out date ({checkOutDate}) must be after Check in date ({checkInDate}).");
            }

            return this.places.SearchPlaces(location, checkInDate, checkOutDate);
        }
    }
}