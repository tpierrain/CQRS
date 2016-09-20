using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.IntegrationModel;
using Newtonsoft.Json;
using Price = BookARoom.Domain.ReadModel.Price;

namespace BookARoom.Infra.Adapters
{
    public class PlaceCatalogFileAdapter : ICatalogPlaces, IProvidePlacesDetails
    {
        private readonly Dictionary<Place, Dictionary<DateTime, List<RoomStatus>>> placesWithPerDateRoomsStatus;

        public PlaceCatalogFileAdapter(string integrationFilesDirectoryPath)
        {
            this.placesWithPerDateRoomsStatus = new Dictionary<Place, Dictionary<DateTime, List<RoomStatus>>>();
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
        }

        public IEnumerable<Place> Places => this.placesWithPerDateRoomsStatus.Keys;

        public string IntegrationFilesDirectoryPath { get; }

        public void LoadPlaceFile(string placeFileNameOrFilePath)
        {
            if (!File.Exists(placeFileNameOrFilePath))
            {
                placeFileNameOrFilePath = Path.Combine(this.IntegrationFilesDirectoryPath, placeFileNameOrFilePath);
            }

            using (var streamReader = File.OpenText(placeFileNameOrFilePath))
            {
                var jsonContent = streamReader.ReadToEnd();
                var integrationFileAvailabilities = JsonConvert.DeserializeObject<RoomsAvailability>(jsonContent);

                AdaptAndStoreIntegrationFileContentForAPlace(integrationFileAvailabilities);
            }
        }

        #region ICatalogPlaces methods

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return from place in this.placesWithPerDateRoomsStatus.Keys
                where place.Location == location
                select place;
        }

        public IEnumerable<Place> SearchAvailablePlacesInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate)
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
                select placeWithAvailabilities.Key).ToList().Distinct();

            return result;
        }

        #endregion

        #region IProvidePlacesDetails

        public PlaceDetails GetDetails(int placeId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region adapter from integration model to domain model

        private void AdaptAndStoreIntegrationFileContentForAPlace(RoomsAvailability integrationFileAvailabilities)
        {
            var place = AdaptPlace(integrationFileAvailabilities.PlaceName, integrationFileAvailabilities.Location);
            var roomsPerDateAvailabilities = AdaptPlaceAvailabilities(integrationFileAvailabilities.AvailabilitiesAt);

            placesWithPerDateRoomsStatus[place] = roomsPerDateAvailabilities;
        }

        private Dictionary<DateTime, List<RoomStatus>> AdaptPlaceAvailabilities(Dictionary<DateTime, RoomStatusAndPrices[]> receivedAvailabilities)
        {
            var result = new Dictionary<DateTime, List<RoomStatus>>();

            foreach (var receivedAvailability in receivedAvailabilities)
            {
                result[receivedAvailability.Key] = AdaptAllRoomsStatusOfThisPlaceForThisDate(receivedAvailabilities);
            }

            return result;
        }

        private static List<RoomStatus> AdaptAllRoomsStatusOfThisPlaceForThisDate(Dictionary<DateTime, RoomStatusAndPrices[]> receivedAvailabilities)
        {
            return (from receivedRoomStatus in receivedAvailabilities.Values
                from roomStatusAndPrices in receivedRoomStatus
                select AdaptRoomStatus(roomStatusAndPrices)).ToList();
        }

        private static RoomStatus AdaptRoomStatus(RoomStatusAndPrices roomStatusAndPrices)
        {
            return new RoomStatus(roomStatusAndPrices.RoomIdentifier, AdaptPrice(roomStatusAndPrices.PriceForOneAdult), AdaptPrice(roomStatusAndPrices.PriceForTwoAdults));
        }

        private static Price AdaptPrice(IntegrationModel.Price price)
        {
            return new Price(price.Currency, price.Value);
        }

        private static Place AdaptPlace(string placeName, string location)
        {
            return new Place(placeName, location);
        }

        #endregion
    }
}