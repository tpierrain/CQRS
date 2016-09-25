using System;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra.ReadModel.Adapters;
using NFluent;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class QueryTheReadModelFacadeTests
    {
        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");

            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);
            var searchQuery = new SearchBookingProposalQuery(checkInDate: DateTime.Now, checkOutDate: DateTime.Now.AddDays(1), location: "Paris", adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            Check.That(bookingProposals).IsEmpty();
        }

        [Test]
        public void Should_find_matching_and_available_place()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");

            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);
            var requestedLocation = "New York";
            var searchQuery = new SearchBookingProposalQuery(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: requestedLocation, adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);

            Check.That(bookingProposals).HasSize(1);

            var bookingProposal = bookingProposals.First();
            Check.That(bookingProposal.Place.Location).IsEqualTo(requestedLocation);
            Check.That(bookingProposal.Place.Name).IsEqualTo("New York Sofitel");
            Check.That(bookingProposal.AvailableRoomsWithPrices).HasSize(3);
        }

        [Test]
        public void Should_find_only_places_that_match_location_and_available_for_this_period()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            placesAdapter.LoadPlaceFile("THE GRAND BUDAPEST HOTEL-availabilities.json"); // available
            placesAdapter.LoadPlaceFile("Danubius Health Spa Resort Helia-availabilities.json"); // available
            placesAdapter.LoadPlaceFile("BudaFull-the-always-unavailable-hotel-availabilities.json"); // unavailable

            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);
            var searchQuery = new SearchBookingProposalQuery(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);

            Check.That(bookingProposals).HasSize(2);
        }

        [Test]
        public void Should_throw_exception_when_checkinDate_is_after_checkOutDate()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);

            Check.ThatCode(() =>
                {
                    var searchQuery = new SearchBookingProposalQuery(checkInDate: DateTime.Now.AddDays(1), checkOutDate: DateTime.Now, location: "Kunming", adultsCount: 1);
                    return readFacade.SearchBookingProposals(searchQuery);
                })
                .Throws<InvalidOperationException>();
        }

        [Test]
        public void Should_find_places_despite_wrong_case_location()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");

            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);
            var searchedLocation = "new york";
            var searchQuery = new SearchBookingProposalQuery(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: searchedLocation, adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);

            Check.That(bookingProposals).HasSize(1);
        }

        [Test]
        public void Should_find_new_matching_places_after_new_place_is_integrated()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);

            // Integrates a first place
            placesAdapter.LoadPlaceFile("THE GRAND BUDAPEST HOTEL-availabilities.json");

            var searchQuery = new SearchBookingProposalQuery(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            Check.That(bookingProposals).HasSize(1);

            // Loads a new place that has available room matching our research
            placesAdapter.LoadPlaceFile("Danubius Health Spa Resort Helia-availabilities.json");
            bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            Check.That(bookingProposals).HasSize(2); // has found one more available place
        }

        [Test]
        public void Should_get_place_from_its_id()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");

            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);

            var placeId = 1;
            var place = readFacade.GetPlace(placeId: placeId);

            Check.That(place.Identifier).IsEqualTo(placeId);
            Check.That(place.Name).IsEqualTo("New York Sofitel");
            Check.That(place.Location).IsEqualTo("New York");
            Check.That(place.NumberOfRooms).IsEqualTo(405);
        }
    }
}
