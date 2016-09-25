using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookARoom.Domain;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra.MessageBus;
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
        // TODO: extract behaviours from this adapter to put it on the domain-side
        private readonly ISubscribeToEvents eventsSubscriber;
        private readonly IProvidePlacesAndRooms repository;

        public PlacesAndRoomsAdapter(string integrationFilesDirectoryPath, ISubscribeToEvents eventsSubscriber)
        {
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
            this.repository = new PlacesAndRoomsRepository();

            this.eventsSubscriber = eventsSubscriber;
            this.eventsSubscriber.RegisterHandler<RoomBooked>(this.Handle); 
            // TODO: question: should we 'functionally subscribe' within the domain code instead?
            // TODO: handle the unsubscription
        }

        private void Handle(RoomBooked roomBooked)
        {
            this.repository.DeclareRoomBooked(roomBooked.PlaceId, roomBooked.RoomNumber, roomBooked.CheckInDate, roomBooked.CheckOutDate);
        }

        public string IntegrationFilesDirectoryPath { get; }

        public IEnumerable<Place> Places => this.repository.Places;

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

        // TODO: get rid of regions by extracting more cohesive types

        #region IProvideRooms methods

        public IEnumerable<BookingProposal> SearchAvailablePlacesInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate)
        {
            return repository.SearchAvailablePlacesInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }

        #endregion

        #region IProvidePlaces

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return repository.SearchFromLocation(location);
        }

        public Place GetPlace(int placeId)
        {
            return repository.GetPlace(placeId);
        }

        #endregion

        #region adapter from integration model to domain model

        private void AdaptAndStoreData(PlaceDetailsWithRoomsAvailabilities dataForThisPlace)
        {
            var place = AdaptPlace(dataForThisPlace.PlaceId, dataForThisPlace.PlaceName, dataForThisPlace.Location, dataForThisPlace.NumberOfRooms);
            this.AdaptAndStoreIntegrationFileContentForAPlace(place, dataForThisPlace);

            this.repository.StorePlace(dataForThisPlace.PlaceId, place);
        }

        private void AdaptAndStoreIntegrationFileContentForAPlace(Place place, PlaceDetailsWithRoomsAvailabilities integrationFileAvailabilitieses)
        {
            var roomsPerDateAvailabilities = AdaptPlaceAvailabilities(integrationFileAvailabilitieses.AvailabilitiesAt);

            this.repository.StorePlaceAvailabilities(place, roomsPerDateAvailabilities);
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