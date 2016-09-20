using System;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Adapters;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class RoomSearchEngineTests
    {
        private DateTime myFavoriteSaturdayIn2017 = new DateTime(2017, 09, 16);

        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var searchEngine = new RoomSearchEngine(new PlaceCatalogFileAdapter(@"../../IntegrationFiles/"));
            var availablePlaces = searchEngine.SearchAvailablePlaceToStay(checkInDate: DateTime.Now, checkOutDate: DateTime.Now.AddDays(1), location: "Paris", adultsCount: 2, roomNumber: 1, childrenCount: 0);
            Assert.AreEqual(0, availablePlaces.Count());
        }

        [Test]
        public void Should_find_matching_and_available_place()
        {
            var places = new PlaceCatalogFileAdapter(@"../../IntegrationFiles/");
            places.LoadPlaceFile("New York Sofitel-availabilities.json");

            var searchEngine = new RoomSearchEngine(places);
            var availablePlaces = searchEngine.SearchAvailablePlaceToStay(myFavoriteSaturdayIn2017, checkOutDate: myFavoriteSaturdayIn2017.AddDays(1), location: "New York", adultsCount: 2, roomNumber: 1, childrenCount: 0);

            Assert.AreEqual(1, availablePlaces.Count());

            var place = availablePlaces.First();
            Assert.AreEqual("New York", place.Location);
            Assert.AreEqual("New York Sofitel", place.Name);
        }

        [Test]
        public void Should_find_only_places_that_match_location_and_available_for_this_period()
        {
            var places = new PlaceCatalogFileAdapter(@"../../IntegrationFiles/");
            places.LoadPlaceFile("THE GRAND BUDAPEST HOTEL-availabilities.json"); // available
            places.LoadPlaceFile("Danubius Health Spa Resort Helia-availabilities.json"); // available
            places.LoadPlaceFile("BudaFull-the-always-unavailable-hotel-availabilities.json"); // unavailable

            var searchEngine = new RoomSearchEngine(places);
            var availablePlaces = searchEngine.SearchAvailablePlaceToStay(myFavoriteSaturdayIn2017, checkOutDate: myFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, roomNumber: 1, childrenCount: 0);

            Assert.AreEqual(2, availablePlaces.Count());
        }

        [Test]
        public void Should_throw_exception_when_checkinDate_is_after_checkOutDate()
        {
            var places = new PlaceCatalogFileAdapter(@"../../IntegrationFiles/");
            var searchEngine = new RoomSearchEngine(places);

            Assert.Throws<InvalidOperationException>( () => searchEngine.SearchAvailablePlaceToStay(checkInDate: DateTime.Now.AddDays(1), checkOutDate: DateTime.Now, location: "Kunming", adultsCount: 1));
        }

        [Test]
        public void Should_find_places_despite_wrong_case_location()
        {
            var places = new PlaceCatalogFileAdapter(@"../../IntegrationFiles/");
            places.LoadPlaceFile("New York Sofitel-availabilities.json");

            var searchEngine = new RoomSearchEngine(places);
            var searchedLocation = "new york";
            var availablePlaces = searchEngine.SearchAvailablePlaceToStay(myFavoriteSaturdayIn2017, checkOutDate: myFavoriteSaturdayIn2017.AddDays(1), location: searchedLocation, adultsCount: 2, roomNumber: 1, childrenCount: 0);

            Assert.AreEqual(1, availablePlaces.Count());
        }

        [Test]
        public void Should_find_new_matching_places_after_new_place_is_integrated()
        {
            var places = new PlaceCatalogFileAdapter(@"../../IntegrationFiles/");
            var searchEngine = new RoomSearchEngine(places);

            // Integrates a first place
            places.LoadPlaceFile("THE GRAND BUDAPEST HOTEL-availabilities.json");
            var availablePlaces = searchEngine.SearchAvailablePlaceToStay(myFavoriteSaturdayIn2017, checkOutDate: myFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, roomNumber: 1, childrenCount: 0);
            Assert.AreEqual(1, availablePlaces.Count());

            // Loads a new place that has available room matching our research
            places.LoadPlaceFile("Danubius Health Spa Resort Helia-availabilities.json");
            availablePlaces = searchEngine.SearchAvailablePlaceToStay(myFavoriteSaturdayIn2017, checkOutDate: myFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, roomNumber: 1, childrenCount: 0);
            Assert.AreEqual(2, availablePlaces.Count()); // find one more available place
        }
    }
}
