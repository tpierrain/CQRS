using System;
using System.Linq;
using System.Threading;
using BookARoom.Domain.ReadModel;
using BookARoom.Domain.WriteModel;
using BookARoom.Infra;
using BookARoom.Infra.Web.MessageBus;
using BookARoom.Infra.Web.ReadModel.Adapters;
using BookARoom.Infra.Web.WriteModel;
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
            var bookingRepository = new BookingAndClientsRepository();
            var bookingHandler = new BookingCommandHandler(new BookingStore(bookingRepository, new FakeBus()));

            Assert.AreEqual(0, bookingRepository.BookingCount);

            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingCommand);

            Assert.AreEqual(1, bookingRepository.GetBookingCommandsFrom("thomas@pierrain.net").Count());
        }

        [Test]
        public void Should_impact_read_model_when_booking_a_room()
        {
            // Initialize Read-model side
            var bus = new FakeBus();
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../integration-files/", bus);
            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");
            var readFacade = new ReadModelFacade(placesAdapter, placesAdapter);

            // Search Rooms availabilities
            var checkInDate = Constants.MyFavoriteSaturdayIn2017;
            var checkOutDate = checkInDate.AddDays(1);

            var searchQuery = new SearchBookingProposalQuery(checkInDate, checkOutDate, location: "New York", adultsCount: 2);
            var bookingProposals = readFacade.SearchBookingProposals(searchQuery);
            // We should get 1 booking proposal with 3 available rooms in it.
            Check.That(bookingProposals).HasSize(1);

            var bookingProposal = bookingProposals.First();
            var initialRoomsNumbers = 3;
            Check.That(bookingProposal.AvailableRoomsWithPrices).HasSize(initialRoomsNumbers);

            // Initialize Write-model side
            var bookingRepository = new BookingAndClientsRepository();
            var bookingHandler = new BookingCommandHandler(new BookingStore(bookingRepository, bus));

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
            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: bookingProposal.Place.Identifier, roomNumber: firstRoomOfTheUniqueProposal.RoomIdentifier, checkInDate: checkInDate, checkOutDate: checkOutDate);
            bookingHandler.Handle(bookingCommand);
        }
    }
}
