using System;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.ReadModel.Adapters;
using BookARoom.Infra.WriteModel;
using Moq;
using NFluent;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class BookingTests
    {
        [Test]
        public void Should_impact_booking_repository_when_sending_a_booking_command()
        {
            var bus = new FakeBus(synchronousPublication:true);
            var bookingRepository = new Mock<ISaveBooking>();
            var clientRepository = new Mock<IHandleClients>();

            CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository.Object, clientRepository.Object, bus, bus);

            bookingRepository.Verify(x => x.Save(It.IsAny<Booking>()), Times.Never);

            var bookingCommand = new BookingCommand(clientId: "thomas@pierrain.net", hotelName:"Super Hotel", hotelId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bus.Send(bookingCommand);

            bookingRepository.Verify(x => x.Save(It.Is<Booking>(y => y.ClientId == "thomas@pierrain.net")), Times.Once);
        }

        [Test]
        public void Should_impact_both_write_and_read_models_when_sending_a_booking_command()
        {
            // Initialize Read-model side
            var bus = new FakeBus(synchronousPublication:true);
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, bus);
            var reservationsAdapter = new ReservationAdapter(bus);
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter, reservationsAdapter, bus);

            // Search Rooms availabilities
            var checkInDate = Constants.MyFavoriteSaturdayIn2017;
            var checkOutDate = checkInDate.AddDays(1);

            var searchQuery = new SearchBookingOptions(checkInDate, checkOutDate, location: "New York", numberOfAdults: 2);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            
            // We should get 1 booking option with 13 available rooms in it.
            Check.That(bookingOptions).HasSize(1);

            var bookingOption = bookingOptions.First();
            var initialRoomsNumbers = 13;
            Check.That(bookingOption.AvailableRoomsWithPrices).HasSize(initialRoomsNumbers);

            // Now, let's book that room!
            var firstRoomOfThisBookingOption = bookingOption.AvailableRoomsWithPrices.First();
            var bookingCommand = new BookingCommand(clientId: "thomas@pierrain.net", hotelName: "New York Sofitel", hotelId: bookingOption.Hotel.Identifier, roomNumber: firstRoomOfThisBookingOption.RoomIdentifier, checkInDate: checkInDate, checkOutDate: checkOutDate);

            // Initialize Write-model side
            var bookingRepository = new BookingAndClientsRepository();
            CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository, bookingRepository, bus, bus);

            // We send the BookARoom command
            bus.Send(bookingCommand);

            // We check that both the BookingRepository (Write model) and the available rooms (Read model) have been updated.
            Check.That(bookingRepository.GetBookingsFrom("thomas@pierrain.net").Count()).IsEqualTo(1);

            // Fetch rooms availabilities now. One room should have disappeared from the search result
            bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            Check.That(bookingOptions).HasSize(1);
            Check.That(bookingOption.AvailableRoomsWithPrices).As("available matching rooms").HasSize(initialRoomsNumbers-1);
        }
    }
}
