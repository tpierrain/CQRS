using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class BookingEngineTests
    {
        [Test]
        public void Should_Book_a_room()
        {
            var bookingStore = new BookingStoreStub();
            var bookingHandler = new BookingCommandHandler(bookingStore);

            Assert.AreEqual(0, bookingStore.BookingCount);

            var bookingRequest = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingRequest);

            Assert.AreEqual(1, bookingStore.BookingCount);
        }
    }

    public class BookingStoreStub : IBookingStore
    {
        public long BookingCount { get; private set; }

        private Dictionary<string, List<ICommand>> perClientCommands;

        public BookingStoreStub()
        {
            this.perClientCommands = new Dictionary<string, List<ICommand>>();
        }

        public void BookARoom(BookARoomCommand bookingRequest)
        {
            if (!this.perClientCommands.ContainsKey(bookingRequest.ClientId))
            {
                this.perClientCommands[bookingRequest.ClientId] = new List<ICommand>();
            }

            this.perClientCommands[bookingRequest.ClientId].Add(bookingRequest);
            this.BookingCount++;
        }
    }

    public interface IBookingStore
    {
        void BookARoom(BookARoomCommand bookingRequest);
    }

    public class BookingCommandHandler
    {
        public BookingStoreStub BookingStore { get; }

        public BookingCommandHandler(BookingStoreStub bookingStore)
        {
            this.BookingStore = bookingStore;
        }

        public void Handle(BookARoomCommand command)
        {
            this.BookingStore.BookARoom(command);

        }
    }

    public class BookARoomCommand : ICommand
    {
        public string ClientId { get; }
        public int PlaceId { get; }
        public string RoomNumber { get; }
        public DateTime CheckInDate { get; }
        public DateTime CheckOutDate { get; }

        public BookARoomCommand(string clientId, int placeId, string roomNumber, DateTime checkInDate, DateTime checkOutDate)
        {
            this.ClientId = clientId;
            this.PlaceId = placeId;
            this.RoomNumber = roomNumber;
            this.CheckInDate = checkInDate;
            this.CheckOutDate = checkOutDate;
        }
    }

    public class ICommand
    {

    }
}
