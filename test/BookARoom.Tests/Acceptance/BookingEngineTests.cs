using System;
using System.Linq;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra.ReadModel.Adapters;
using BookARoom.Infra.WriteModel;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class BookingEngineTests
    {
        [Test]
        public void Should_Book_a_room()
        {
            var bookingRepository = new BookingAndClientsRepository();
            var bookingHandler = new BookingCommandHandler(new BookingStore(bookingRepository));

            Assert.AreEqual(0, bookingRepository.BookingCount);

            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingCommand);

            Assert.AreEqual(1, bookingRepository.GetBookingCommandsFrom("thomas@pierrain.net").Count());
        }

        [Test]
        public void Should_impact_read_model_when_booking_a_room()
        {
            // Initialize Read-model side
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");
            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");
            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);

            // Search Rooms availabilities
            var checkInDate = Constants.MyFavoriteSaturdayIn2017;
            var checkOutDate = checkInDate.AddDays(1);

            var bookingProposals = readFacade.SearchBookingProposals(checkInDate, checkOutDate, location: "New York", adultsCount: 2);
            // We should get 1 booking proposal with 3 available rooms in it.
            Assert.AreEqual(1, bookingProposals.Count());

            var bookingProposal = bookingProposals.First();
            Assert.AreEqual(3, bookingProposal.AvailableRoomsWithPrices.Count());

            // Initialize Write-model side
            var bookingRepository = new BookingAndClientsRepository();
            var bookingHandler = new BookingCommandHandler(new BookingStore(bookingRepository));

            // We book a room from that booking proposal
            BookARoomFromAProposal(bookingProposal, checkInDate, checkOutDate, bookingHandler);
            Assert.AreEqual(1, bookingRepository.GetBookingCommandsFrom("thomas@pierrain.net").Count());

            // Fetch rooms availabilities now. One room should have disappeared
            bookingProposals = readFacade.SearchBookingProposals(checkInDate, checkOutDate, location: "New York", adultsCount: 2);
            Assert.AreEqual(1, bookingProposals.Count());
            Assert.AreEqual(3-1, bookingProposal.AvailableRoomsWithPrices.Count());
        }

        private static void BookARoomFromAProposal(BookingProposal bookingProposal, DateTime checkInDate, DateTime checkOutDate, BookingCommandHandler bookingHandler)
        {
            var firstRoomOfTheUniqueProposal = bookingProposal.AvailableRoomsWithPrices.First();
            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: bookingProposal.Place.Identifier, roomNumber: firstRoomOfTheUniqueProposal.RoomIdentifier, checkInDate: checkInDate, checkOutDate: checkOutDate);
            bookingHandler.Handle(bookingCommand);
        }
    }
}
