using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.IntegrationModel;
using Newtonsoft.Json;
using Price = BookARoom.Domain.ReadModel.Price;

namespace BookARoom.Infra.ReadModel.Adapters
{
    /// <summary>
    /// Adapter between the Integration model (json external files) and the domain one.
    /// <remarks>Implementation of the Ports and Adapters patterns (a.k.a. hexagonal architecture).</remarks>
    /// </summary>
    public class HotelsAndRoomsAdapter : IProvideRooms, IProvideHotel
    {
        // TODO: extract behaviours from this adapter to put it on the domain-side
        private readonly ISubscribeToEvents eventsSubscriber;
        private readonly IStoreAndProvideHotelsAndRooms repository;

        public HotelsAndRoomsAdapter(string integrationFilesDirectoryPath, ISubscribeToEvents eventsSubscriber)
        {
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
            this.repository = new HotelsAndRoomsRepository();

            this.eventsSubscriber = eventsSubscriber;
            this.eventsSubscriber.RegisterHandler<RoomBooked>(this.Handle); 
            // TODO: question: should we 'functionally subscribe' within the domain code instead?
            // TODO: handle the unsubscription
        }

        private void Handle(RoomBooked roomBooked)
        {
            this.repository.DeclareRoomBooked(roomBooked.HotelId, roomBooked.RoomNumber, roomBooked.CheckInDate, roomBooked.CheckOutDate);
        }

        public string IntegrationFilesDirectoryPath { get; }

        public IEnumerable<Hotel> Hotels => this.repository.Hotels;

        public void LoadHotelFile(string hotelFileNameOrFilePath)
        {
            if (!File.Exists(hotelFileNameOrFilePath))
            {
                hotelFileNameOrFilePath = Path.Combine(this.IntegrationFilesDirectoryPath, hotelFileNameOrFilePath);
            }

            var integrationModelForThisHotel = GetIntegrationModelForThisHotel(hotelFileNameOrFilePath);

            this.AdaptAndStoreData(integrationModelForThisHotel);
        }

        public void LoadAllHotelsFiles()
        {
            var filesNames = Directory.GetFiles(this.IntegrationFilesDirectoryPath);
            foreach (var fileName in filesNames)
            {
                LoadHotelFile(fileName);
            }
        }

        private static HotelDetailsWithRoomsAvailabilities GetIntegrationModelForThisHotel(string hotelFileNameOrFilePath)
        {
            IntegrationModel.HotelDetailsWithRoomsAvailabilities integrationFileAvailabilitieses = null;
            using (var streamReader = File.OpenText(hotelFileNameOrFilePath))
            {
                var jsonContent = streamReader.ReadToEnd();
                integrationFileAvailabilitieses = JsonConvert.DeserializeObject<HotelDetailsWithRoomsAvailabilities>(jsonContent);
            }
            return integrationFileAvailabilitieses;
        }

        // TODO: get rid of regions by extracting more cohesive types

        #region IProvideRooms methods

        public IEnumerable<BookingOption> SearchAvailableHotelsInACaseInsensitiveWay(string location, DateTime checkInDate, DateTime checkOutDate)
        {
            return repository.SearchAvailableHotelsInACaseInsensitiveWay(location, checkInDate, checkOutDate);
        }

        #endregion

        #region IProvideHotel

        public IEnumerable<Hotel> SearchFromLocation(string location)
        {
            return repository.SearchFromLocation(location);
        }

        public Hotel GetHotel(int hotelId)
        {
            return repository.GetHotel(hotelId);
        }

        #endregion

        #region adapter from integration model to domain model

        private void AdaptAndStoreData(HotelDetailsWithRoomsAvailabilities dataForThisHotel)
        {
            var hotel = AdaptHotel(dataForThisHotel.HotelId, dataForThisHotel.HotelName, dataForThisHotel.Location, dataForThisHotel.NumberOfRooms);
            this.AdaptAndStoreIntegrationFileContentForAnHotel(hotel, dataForThisHotel);

            this.repository.StoreHotel(dataForThisHotel.HotelId, hotel);
        }

        private void AdaptAndStoreIntegrationFileContentForAnHotel(Hotel hotel, HotelDetailsWithRoomsAvailabilities integrationFileAvailabilitieses)
        {
            var roomsPerDateAvailabilities = AdaptHotelAvailabilities(integrationFileAvailabilitieses.AvailabilitiesAt);

            this.repository.StoreHotelAvailabilities(hotel, roomsPerDateAvailabilities);
        }

        private Dictionary<DateTime, List<RoomWithPrices>> AdaptHotelAvailabilities(Dictionary<DateTime, RoomStatusAndPrices[]> receivedAvailabilities)
        {
            var result = new Dictionary<DateTime, List<RoomWithPrices>>();

            foreach (var receivedAvailability in receivedAvailabilities)
            {
                result[receivedAvailability.Key] = AdaptAllRoomsStatusOfThisHotelForThisDate(receivedAvailabilities);
            }

            return result;
        }

        private static List<RoomWithPrices> AdaptAllRoomsStatusOfThisHotelForThisDate(Dictionary<DateTime, RoomStatusAndPrices[]> receivedAvailabilities)
        {
            return (from receivedRoomStatus in receivedAvailabilities.Values
                from roomStatusAndPrices in receivedRoomStatus
                select AdaptRoomStatus(roomStatusAndPrices)).ToList();
        }

        private static RoomWithPrices AdaptRoomStatus(RoomStatusAndPrices roomStatusAndPrices)
        {
            return new RoomWithPrices(roomStatusAndPrices.RoomIdentifier, AdaptPrice(roomStatusAndPrices.OneAdultOccupancyPrice), AdaptPrice(roomStatusAndPrices.TwoAdultsOccupancyPrice));
        }

        private static Price AdaptPrice(IntegrationModel.Price price)
        {
            return new Price(price.Currency, price.Value);
        }

        private static Hotel AdaptHotel(int hotelId, string hotelName, string location, int numberOfRooms)
        {
            return new Hotel(hotelId, hotelName, location, numberOfRooms);
        }

        #endregion
    }
}