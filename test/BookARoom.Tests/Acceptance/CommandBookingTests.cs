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
    public class CommandBookingTests
    {
        [Test]
        public void Should_Book_a_room()
        {
            var bookingRepository = new Mock<IBookingRepository>();
            var clientRepository = new Mock<IClientRepository>();
            var bookingHandler = new BookingCommandHandler(new BookingStore(bookingRepository.Object, clientRepository.Object, new FakeBus()));

            bookingRepository.Verify(x => x.Save(It.IsAny<BookARoomCommand>()), Times.Never);

            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", hotelId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingCommand);

            bookingRepository.Verify(x => x.Save(It.Is<BookARoomCommand>(y => y.ClientId == "thomas@pierrain.net")), Times.Once);
        }

        [Test]
        public void Should_impact_bookingOptions_read_model_when_booking_a_room()
        {
            // Initialize Read-model side
            var bus = new FakeBus();
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, bus);
            var reservationsAdapter = new ReservationAdapter(bus);
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter, reservationsAdapter, bus);

            // Search Rooms availabilities
            var checkInDate = Constants.MyFavoriteSaturdayIn2017;
            var checkOutDate = checkInDate.AddDays(1);

            var searchQuery = new SearchBookingOptions(checkInDate, checkOutDate, location: "New York", numberOfAdults: 2);
            var bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            // We should get 1 booking option with 3 available rooms in it.
            Check.That(bookingOptions).HasSize(1);

            var bookingOption = bookingOptions.First();
            var initialRoomsNumbers = 3;
            Check.That(bookingOption.AvailableRoomsWithPrices).HasSize(initialRoomsNumbers);

            // Initialize Write-model side
            var bookingRepository = new BookingAndClientsRepository();

            var bookingHandler = CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository, bookingRepository, bus, bus);

            // We book a room from that booking option
            BookAnOption(bookingOption, checkInDate, checkOutDate, bookingHandler);
            Check.That(bookingRepository.GetBookingCommandsFrom("thomas@pierrain.net").Count()).IsEqualTo(1);

            // Fetch rooms availabilities now. One room should have disappeared
            bookingOptions = readFacade.SearchBookingOptions(searchQuery);
            
            Check.That(bookingOptions).HasSize(1);
            Check.That(bookingOption.AvailableRoomsWithPrices).As("available matching rooms").HasSize(initialRoomsNumbers-1);
        }

        private static void BookAnOption(BookingOption bookingOption, DateTime checkInDate, DateTime checkOutDate, BookingCommandHandler bookingHandler)
        {
            var firstRoomOfTheUniqueOption = bookingOption.AvailableRoomsWithPrices.First();
            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", hotelId: bookingOption.Hotel.Identifier, roomNumber: firstRoomOfTheUniqueOption.RoomIdentifier, checkInDate: checkInDate, checkOutDate: checkOutDate);
            bookingHandler.Handle(bookingCommand);
        }
    }
}
