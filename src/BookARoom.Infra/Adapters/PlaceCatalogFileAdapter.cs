using System;
using System.Collections.Generic;
using System.IO;
using BookARoom.Domain;
using BookARoom.Integration;
using Newtonsoft.Json;

namespace BookARoom.Infra.Adapters
{
    public class PlaceCatalogFileAdapter : ICatalogPlaces
    {
        private Dictionary<Place, Dictionary<DateTime, List<RoomStatus>>> placesWithPerDateRoomStatus;

        public PlaceCatalogFileAdapter(string integrationFilesDirectoryPath)
        {
            this.placesWithPerDateRoomStatus = new Dictionary<Place, Dictionary<DateTime, List<RoomStatus>>>();
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
        }

        public IEnumerable<Place> Places => this.placesWithPerDateRoomStatus.Keys;

        public string IntegrationFilesDirectoryPath { get; }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            throw new NotImplementedException();
        }

        public void LoadPlaceFile(string placeFilePath)
        {
            if (!File.Exists(placeFilePath))
            {
                placeFilePath = Path.Combine(this.IntegrationFilesDirectoryPath, placeFilePath);
            }

            using (var streamReader = File.OpenText(placeFilePath))
            {
                var jsonContent = streamReader.ReadToEnd();
                var availabilities = JsonConvert.DeserializeObject<RoomsAvailability>(jsonContent);

                var emptyLocation = string.Empty;

                placesWithPerDateRoomStatus[AdaptPlace(availabilities.PlaceName, emptyLocation)] = AdaptPlaceAvailabilities(availabilities.AvailabilitiesAt);
            }
        }

        private Dictionary<DateTime, List<RoomStatus>> AdaptPlaceAvailabilities(Dictionary<DateTime, Integration.RoomStatusAndPrices[]> receivedAvailabilities)
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
            var roomsStatusForThisDateAtThisPlace = new List<RoomStatus>();
            foreach (RoomStatusAndPrices[] receivedRoomStatus in receivedAvailabilities.Values)
            {
                foreach (RoomStatusAndPrices roomStatusAndPrices in receivedRoomStatus)
                {
                    // Adapt room status
                    var roomStatus = AdaptRoomStatus(roomStatusAndPrices);
                    roomsStatusForThisDateAtThisPlace.Add(roomStatus);
                }
            }

            return roomsStatusForThisDateAtThisPlace;
        }

        private static Domain.RoomStatus AdaptRoomStatus(Integration.RoomStatusAndPrices roomStatusAndPrices)
        {
            return new RoomStatus(roomStatusAndPrices.RoomIdentifier, AdaptPrice(roomStatusAndPrices.PriceForOneAdult), AdaptPrice(roomStatusAndPrices.PriceForTwoAdults));
        }

        private static Domain.Price AdaptPrice(Integration.Price price)
        {
            return new Domain.Price(price.Currency, price.Value);
        }

        private static Place AdaptPlace(string placeName, string emptyLocation)
        {
            return new Place(placeName, emptyLocation);
        }
    }
}