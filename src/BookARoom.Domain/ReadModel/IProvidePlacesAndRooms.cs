using System;
using System.Collections.Generic;

namespace BookARoom.Domain.ReadModel
{
    public interface IProvidePlacesAndRooms
    {
        Place GetPlace(int placeId);
        IEnumerable<Place> Places { get; }
        IEnumerable<Place> SearchFromLocation(string location);

        IEnumerable<BookingProposal> SearchAvailablePlacesInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate);

        void StorePlace(int placeId, Place place);
        void StorePlaceAvailabilities(Place place, Dictionary<DateTime, List<RoomWithPrices>> perDateRoomsAvailabilities);
    }
}
