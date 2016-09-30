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
    public class CommandRoomsBookingTests
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
        public void Should_impact_read_model_when_booking_a_room()
        {
            // Initialize Read-model side
            var bus = new FakeBus();
            var hotelsAdapter = new HotelAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, bus);
            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");

            var readFacade = CompositionRootHelper.BuildTheReadModelHexagon(hotelsAdapter, hotelsAdapter);

            // Search Rooms availabilities
            var checkInDate = Constants.MyFavoriteSaturdayIn2017;
            var checkOutDate = checkInDate.AddDays(1);

            var searchQuery = new SearchBookingProposal(checkInDate, checkOutDate, location: "New York", adultsCount: 2);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            // We should get 1 booking proposal with 3 available rooms in it.
            Check.That(bookingProposals).HasSize(1);

            var bookingProposal = bookingProposals.First();
            var initialRoomsNumbers = 3;
            Check.That(bookingProposal.AvailableRoomsWithPrices).HasSize(initialRoomsNumbers);

            // Initialize Write-model side
            var bookingRepository = new BookingAndClientsRepository();

            var bookingHandler = CompositionRootHelper.BuildTheWriteModelHexagon(bookingRepository, bookingRepository, bus);

            // We book a room from that booking proposal
            BookARoomFromAProposal(bookingProposal, checkInDate, checkOutDate, bookingHandler);
            Check.That(bookingRepository.GetBookingCommandsFrom("thomas@pierrain.net").Count()).IsEqualTo(1);

            // Fetch rooms availabilities now. One room should have disappeared
            bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            
            Check.That(bookingProposals).HasSize(1);
            Check.That(bookingProposal.AvailableRoomsWithPrices).As("available matching rooms").HasSize(initialRoomsNumbers-1);
        }

        private static void BookARoomFromAProposal(BookingProposal bookingProposal, DateTime checkInDate, DateTime checkOutDate, BookingCommandHandler bookingHandler)
        {
            var firstRoomOfTheUniqueProposal = bookingProposal.AvailableRoomsWithPrices.First();
            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", hotelId: bookingProposal.Hotel.Identifier, roomNumber: firstRoomOfTheUniqueProposal.RoomIdentifier, checkInDate: checkInDate, checkOutDate: checkOutDate);
            bookingHandler.Handle(bookingCommand);
        }
    }
}
