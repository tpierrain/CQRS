using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.IntegrationModel;
using Newtonsoft.Json;
using Price = BookARoom.Domain.ReadModel.Price;

namespace BookARoom.Infra.ReadModel.Adapters
{
    /// <summary>
    /// Adapter between the Integration model (json external files) and the domain one.
    /// <remarks>Implementation of the Ports and Adapters patterns (a.k.a. hexagonal architecture).</remarks>
    /// </summary>
    public class PlacesAndRoomsAdapter : IProvideRooms, IProvidePlaces
    {
        private readonly IPlacesAndRoomsRepository readModelDatabase;

        public PlacesAndRoomsAdapter(string integrationFilesDirectoryPath)
        {
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
            this.readModelDatabase = new ReadModelDatabase();
        }

        public string IntegrationFilesDirectoryPath { get; }

        public IEnumerable<Place> Places => this.readModelDatabase.Places;

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

        #region IProvideRooms methods

        public IEnumerable<BookingProposal> SearchAvailablePlacesInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate)
        {
            return readModelDatabase.SearchAvailablePlacesInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }

        #endregion

        #region IProvidePlaces

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return readModelDatabase.SearchFromLocation(location);
        }

        public Place GetPlace(int placeId)
        {
            return readModelDatabase.GetPlace(placeId);
        }

        #endregion

        #region adapter from integration model to domain model

        private void AdaptAndStoreData(PlaceDetailsWithRoomsAvailabilities dataForThisPlace)
        {
            var place = AdaptPlace(dataForThisPlace.PlaceId, dataForThisPlace.PlaceName, dataForThisPlace.Location, dataForThisPlace.NumberOfRooms);
            this.AdaptAndStoreIntegrationFileContentForAPlace(place, dataForThisPlace);

            this.readModelDatabase.StorePlace(dataForThisPlace.PlaceId, place);
        }

        private void AdaptAndStoreIntegrationFileContentForAPlace(Place place, PlaceDetailsWithRoomsAvailabilities integrationFileAvailabilitieses)
        {
            var roomsPerDateAvailabilities = AdaptPlaceAvailabilities(integrationFileAvailabilitieses.AvailabilitiesAt);

            this.readModelDatabase.StorePlaceAvailabilities(place, roomsPerDateAvailabilities);
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