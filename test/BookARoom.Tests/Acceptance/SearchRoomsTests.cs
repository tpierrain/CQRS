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
    public class SearchRoomsTests
    {
        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var searchQuery = new SearchBookingOptions(checkInDate: DateTime.Now, checkOutDate: DateTime.Now.AddDays(1), location: "Paris", numberOfAdults: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            Check.That(bookingOptions).IsNotNull().And.IsEmpty();
        }

        [Test]
        public void Should_find_matching_and_available_hotels()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            var requestedLocation = "New York";
            var searchQuery = new SearchBookingOptions(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: requestedLocation, numberOfAdults: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);

            Check.That(bookingOptions).HasSize(1);

            var bookingOption = bookingOptions.First();
            Check.That(bookingOption.Hotel.Location).IsEqualTo(requestedLocation);
            Check.That(bookingOption.Hotel.Name).IsEqualTo("New York Sofitel");
            Check.That(bookingOption.AvailableRoomsWithPrices).HasSize(13);
        }

        [Test]
        public void Should_find_only_hotels_that_match_location_and_availability_for_this_period()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("THE GRAND BUDAPEST HOTEL-availabilities.json"); // available
            hotelsAdapter.LoadHotelFile("Danubius Health Spa Resort Helia-availabilities.json"); // available
            hotelsAdapter.LoadHotelFile("BudaFull-the-always-unavailable-hotel-availabilities.json"); // unavailable

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var searchQuery = new SearchBookingOptions(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", numberOfAdults: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);

            Check.That(bookingOptions).HasSize(2);
        }

        [Test]
        public void Should_throw_exception_when_checkinDate_is_after_checkOutDate()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            Check.ThatCode(() =>
                {
                    var searchQuery = new SearchBookingOptions(checkInDate: DateTime.Now.AddDays(1), checkOutDate: DateTime.Now, location: "Kunming", numberOfAdults: 1);
                    return readFacade.SearchBookingOptions(searchQuery);
                })
                .Throws<InvalidOperationException>();
        }

        [Test]
        public void Should_find_hotels_despite_incorrect_case_for_location()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);
            var searchedLocation = "new york";
            var searchQuery = new SearchBookingOptions(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: searchedLocation, numberOfAdults: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);

            Check.That(bookingOptions).HasSize(1);
        }

        [Test]
        public void Should_find_one_more_matching_hotel_after_new_hotel_is_integrated()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            // Integrates a first hotel
            hotelsAdapter.LoadHotelFile("THE GRAND BUDAPEST HOTEL-availabilities.json");

            var searchQuery = new SearchBookingOptions(Constants.MyFavoriteSaturdayIn2017, checkOutDate: Constants.MyFavoriteSaturdayIn2017.AddDays(1), location: "Budapest", numberOfAdults: 2, numberOfRoomsNeeded: 1, childrenCount: 0);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            Check.That(bookingOptions).HasSize(1);

            // Loads a new hotel that has available room matching our research
            hotelsAdapter.LoadHotelFile("Danubius Health Spa Resort Helia-availabilities.json");
            bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            Check.That(bookingOptions).HasSize(2); // has found one more available hotel
        }

        [Test]
        public void Should_get_hotel_from_its_id()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());
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
