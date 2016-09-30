using System;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Infra;
using BookARoom.Infra.MessageBus;
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
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var searchQuery = new SearchBookingProposal(checkInDate: DateTime.Now, checkOutDate: DateTime.Now.AddDays(1), location: "Paris", adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            Check.That(bookingProposals).IsEmpty();
        }

        [Test]
        public void Should_find_matching_and_available_hotel()
        {
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var requestedLocation = "New York";
            var searchQuery = new SearchBookingProposal(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: requestedLocation, adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);

            Check.That(bookingProposals).HasSize(1);

            var bookingProposal = bookingProposals.First();
            Check.That(bookingProposal.Hotel.Location).IsEqualTo(requestedLocation);
            Check.That(bookingProposal.Hotel.Name).IsEqualTo("New York Sofitel");
            Check.That(bookingProposal.AvailableRoomsWithPrices).HasSize(3);
        }

        [Test]
        public void Should_find_only_hotels_that_match_location_and_available_for_this_period()
        {
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("THE GRAND BUDAPEST HOTEL-availabilities.json"); // available
            hotelsAdapter.LoadHotelFile("Danubius Health Spa Resort Helia-availabilities.json"); // available
            hotelsAdapter.LoadHotelFile("BudaFull-the-always-unavailable-hotel-availabilities.json"); // unavailable

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var searchQuery = new SearchBookingProposal(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);

            Check.That(bookingProposals).HasSize(2);
        }

        [Test]
        public void Should_throw_exception_when_checkinDate_is_after_checkOutDate()
        {
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            Check.ThatCode(() =>
                {
                    var searchQuery = new SearchBookingProposal(checkInDate: DateTime.Now.AddDays(1), checkOutDate: DateTime.Now, location: "Kunming", adultsCount: 1);
                    return readFacade.SearchBookingProposals(searchQuery);
                })
                .Throws<InvalidOperationException>();
        }

        [Test]
        public void Should_find_hotels_despite_wrong_case_location()
        {
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var searchedLocation = "new york";
            var searchQuery = new SearchBookingProposal(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: searchedLocation, adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);

            Check.That(bookingProposals).HasSize(1);
        }

        [Test]
        public void Should_find_new_matching_hotels_after_new_hotel_is_integrated()
        {
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            // Integrates a first hotel
            hotelsAdapter.LoadHotelFile("THE GRAND BUDAPEST HOTEL-availabilities.json");

            var searchQuery = new SearchBookingProposal(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", adultsCount: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            Check.That(bookingProposals).HasSize(1);

            // Loads a new hotel that has available room matching our research
            hotelsAdapter.LoadHotelFile("Danubius Health Spa Resort Helia-availabilities.json");
            bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            Check.That(bookingProposals).HasSize(2); // has found one more available hotel
        }

        [Test]
        public void Should_get_hotel_from_its_id()
        {
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            var hotelId = 1;
            var hotel = readFacade.GetHotel(hotelId: hotelId);

            Check.That(hotel.Identifier).IsEqualTo(hotelId);
            Check.That(hotel.Name).IsEqualTo("New York Sofitel");
            Check.That(hotel.Location).IsEqualTo("New York");
            Check.That(hotel.NumberOfRooms).IsEqualTo(405);
        }
    }
}
