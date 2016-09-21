using System;
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
            var bookingStore = new WriteModelDatabase();
            var bookingHandler = new BookingCommandHandler(bookingStore);

            Assert.AreEqual(0, bookingStore.BookingCount);

            var bookingRequest = new BookARoomCommand(clientId: "thomas@pierrain.net", placeId: 1, roomNumber: "2", checkInDate: DateTime.Parse("2016-09-17"), checkOutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingRequest);

            Assert.AreEqual(1, bookingStore.BookingCount);
        }
    }
}
