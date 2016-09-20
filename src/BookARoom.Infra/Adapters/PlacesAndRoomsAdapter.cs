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
    /// <summary>
    /// Adapter between the Integration model (json external files) and the domain one.
    /// <remarks>Implementation of the Ports and Adapters patterns (a.k.a. hexagonal architecture).</remarks>
    /// </summary>
    public class PlacesAndRoomsAdapter : IProvideRooms, IProvidePlaces
    {
        private readonly Dictionary<Place, Dictionary<DateTime, List<RoomWithPrices>>> placesWithPerDateRoomsStatus;
        private readonly Dictionary<int, Place> placesPerId = new Dictionary<int, Place>();

        public PlacesAndRoomsAdapter(string integrationFilesDirectoryPath)
        {
            this.placesWithPerDateRoomsStatus = new Dictionary<Place, Dictionary<DateTime, List<RoomWithPrices>>>();
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

            var externalDataForThisPlace = GetIntegrationModelForThisPlace(placeFileNameOrFilePath);

            this.AdaptAndStoreData(externalDataForThisPlace);
        }
        
        private static PlaceDetailsWithRoomsAvailabilities GetIntegrationModelForThisPlace(string placeFileNameOrFilePath)
        {
            IntegrationModel.PlaceDetailsWithRoomsAvailabilities integrationFileAvailabilitieses = null;
            using (var streamReader = File.OpenText(placeFileNameOrFilePath))
            {
                var jsonContent = streamReader.ReadToEnd();
                integrationFileAvailabilitieses = JsonConvert.DeserializeObject<PlaceDetailsWithRoomsAvailabilities>(jsonContent);
            }
            return integrationFileAvailabilitieses;
        }

        private void StorePlaceWithId(int placeId, Place place)
        {
            this.placesPerId[placeId] = place;
        }

        #region IProvideRooms methods

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

        #endregion

        #region IProvidePlaces

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

        #endregion

        #region adapter from integration model to domain model

        private void AdaptAndStoreData(PlaceDetailsWithRoomsAvailabilities dataForThisPlace)
        {
            var place = AdaptPlace(dataForThisPlace.PlaceId, dataForThisPlace.PlaceName, dataForThisPlace.Location, dataForThisPlace.NumberOfRooms);
            this.AdaptAndStoreIntegrationFileContentForAPlace(place, dataForThisPlace);
            this.StorePlaceWithId(dataForThisPlace.PlaceId, place);
        }

        private void AdaptAndStoreIntegrationFileContentForAPlace(Place place, PlaceDetailsWithRoomsAvailabilities integrationFileAvailabilitieses)
        {
            var roomsPerDateAvailabilities = AdaptPlaceAvailabilities(integrationFileAvailabilitieses.AvailabilitiesAt);

            placesWithPerDateRoomsStatus[place] = roomsPerDateAvailabilities;
        }

        private Dictionary<DateTime, List<RoomWithPrices>> AdaptPlaceAvailabilities(Dictionary<DateTime, RoomStatusAndPrices[]> receivedAvailabilities)
        {
            var result = new Dictionary<DateTime, List<RoomWithPrices>>();

            foreach (var receivedAvailability in receivedAvailabilities)
            {
                result[receivedAvailability.Key] = AdaptAllRoomsStatusOfThisPlaceForThisDate(receivedAvailabilities);
            }

            return result;
        }

        private static List<RoomWithPrices> AdaptAllRoomsStatusOfThisPlaceForThisDate(Dictionary<DateTime, RoomStatusAndPrices[]> receivedAvailabilities)
        {
            return (from receivedRoomStatus in receivedAvailabilities.Values
                from roomStatusAndPrices in receivedRoomStatus
                select AdaptRoomStatus(roomStatusAndPrices)).ToList();
        }

        private static RoomWithPrices AdaptRoomStatus(RoomStatusAndPrices roomStatusAndPrices)
        {
            return new RoomWithPrices(roomStatusAndPrices.RoomIdentifier, AdaptPrice(roomStatusAndPrices.PriceForOneAdult), AdaptPrice(roomStatusAndPrices.PriceForTwoAdults));
        }

        private static Price AdaptPrice(IntegrationModel.Price price)
        {
            return new Price(price.Currency, price.Value);
        }

        private static Place AdaptPlace(int placeId, string placeName, string location, int numberOfRooms)
        {
            return new Place(placeId, placeName, location, numberOfRooms);
        }

        #endregion
    }
}