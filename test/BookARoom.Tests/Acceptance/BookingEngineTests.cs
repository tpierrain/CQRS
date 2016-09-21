using System;
using System.Linq;
using BookARoom.Domain.WriteModel;
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
            var bookingDatabase = new WriteModelDatabase();
            var bookingHandler = new BookingCommandHandler(new BookingStore(bookingDatabase));

            Assert.AreEqual(0, bookingDatabase.BookingCount);

            var bookingCommand = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingCommand);

            Assert.AreEqual(1, bookingDatabase.GetBookingCommandsFrom("thomas@pierrain.net").Count());
        }
    }
}
