using System;
using System.Collections.Generic;
using System.Linq;
using BookARoom.Domain.ReadModel;

namespace BookARoom.Infra.ReadModel
{
    public class ReadModelDatabase : IPlacesAndRoomsRepository
    {
        public readonly Dictionary<Place, Dictionary<DateTime, List<RoomWithPrices>>> placesWithPerDateRoomsStatus;
        private readonly Dictionary<int, Place> placesPerId = new Dictionary<int, Place>();

        public ReadModelDatabase()
        {
            this.placesWithPerDateRoomsStatus = new Dictionary<Place, Dictionary<DateTime, List<RoomWithPrices>>>();
        }

        public IEnumerable<Place> Places => this.placesWithPerDateRoomsStatus.Keys;

        public IEnumerable<BookingProposal> SearchAvailablePlacesInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate)
        {
            var result = (from placeWithAvailabilities in this.placesWithPerDateRoomsStatus
                from dateAndRooms in this.placesWithPerDateRoomsStatus.Values
                from date in dateAndRooms.Keys
                from availableRooms in dateAndRooms.Values
                where string.Equals(placeWithAvailabilities.Key.Location, location, StringComparison.CurrentCultureIgnoreCase)
                      && (date >= checkInDate && date <= checkOutDate)
                      && availableRooms.Count > 0
                      && dateAndRooms.Values.Contains(availableRooms)
                      && placeWithAvailabilities.Value == dateAndRooms
                select new BookingProposal(placeWithAvailabilities.Key, availableRooms) ).ToList().Distinct();

            return result;
        }

        public void StorePlaceAvailabilities(Place place, Dictionary<DateTime, List<RoomWithPrices>> perDateRoomsAvailabilities)
        {
            this.placesWithPerDateRoomsStatus[place] = perDateRoomsAvailabilities;
        }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return from place in this.placesWithPerDateRoomsStatus.Keys
                where place.Location == location
                select place;
        }

        public Place GetPlace(int placeId)
        {
            return this.placesPerId[placeId];
        }

        public void StorePlace(int placeId, Place place)
        {
            this.placesPerId[placeId] = place;
        }
    }
}