using System;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class BookingEngineTests
    {
        [Test]
        public void Should_Book_an_available_room()
        {
            var bookingStore = new BookingStore();
            var bookingHandler = new BookingCommandHandler(bookingStore);

            Assert.AreEqual(0, bookingStore.BookingCount);

            var bookingRequest = new BookARoomCommand(placeId: 1, roomNumber: "2", checkinDate: DateTime.Parse("2016-09-17"), checkoutDate: DateTime.Parse("2016-09-18"));
            bookingHandler.Handle(bookingRequest);

            Assert.AreEqual(1, bookingStore.BookingCount);
        }
    }

    public class BookingStore
    {
        public long BookingCount { get; }


    }

    public class BookingCommandHandler
    {
        public BookingStore BookingStore { get; }

        public BookingCommandHandler(BookingStore bookingStore)
        {
            this.BookingStore = bookingStore;
        }

        public void Handle(BookARoomCommand command)
        {
           
        }
    }

    public class BookARoomCommand
    {
        public int PlaceId { get; }
        public string RoomNumber { get; }
        public DateTime CheckinDate { get; }
        public DateTime CheckoutDate { get; set; }

        public BookARoomCommand(int placeId, string roomNumber, DateTime checkinDate, DateTime checkoutDate)
        {
            this.PlaceId = placeId;
            this.RoomNumber = roomNumber;
            this.CheckinDate = checkinDate;
            CheckoutDate = checkoutDate;
        }
    }
}
