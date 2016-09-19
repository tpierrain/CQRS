using System;
using NUnit.Framework;
using System.Linq;
using BookARoom.Domain;
using BookARoom.Infra.Adapters;

namespace BookARoom.Tests
{
    [TestFixture]
    public partial class RoomBookingEngineTests
    {
        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var bookingEngine = new RoomBookingEngine(new PlaceCatalog());
            var availablePlaces = bookingEngine.SearchPlaceToStay(checkInDate:DateTime.Now, checkOutDate:DateTime.Now.AddDays(1), location:"Paris", roomNumber:1, adultsCount:2, childrenCount:0);
            Assert.AreEqual(0, availablePlaces.Count());
        }

        [Test]
        public void Should_find_places_when_one_match/*_and_is_available*/()
        {
            var places = new PlaceCatalogFileAdapter(@"../../IntegrationFiles/");
            places.LoadPlaceFile("New York Sofitel-availabilities.json");

            var bookingEngine = new RoomBookingEngine(places);
            var checkInDate = new DateTime(2016, 09, 11);
            var availablePlaces = bookingEngine.SearchPlaceToStay(checkInDate, checkOutDate: checkInDate.AddDays(1), location: "New York", roomNumber: 1, adultsCount: 2, childrenCount: 0);

            Assert.AreEqual(1, availablePlaces.Count());
        }
    }
}
