namespace RoomsBooking.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class RoomBookingEngineTests
    {
        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var bookingEngine = new RoomBookingEngine();
            IEnumerable<Place> availablePlaces = bookingEngine.SearchPlaceToStay(checkInDate:DateTime.Now, checkOutDate:DateTime.Now.AddDays(1), location:"Paris", roomNumber:1, adultsCout:2, childrenCount:0);
            Assert.AreEqual(0, availablePlaces.Count());
        }
    }

    public class RoomBookingEngine
    {
        public IEnumerable<Place> SearchPlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int roomNumber, int adultsCout, int childrenCount)
        {
            throw new NotImplementedException();
        }
    }

    public class Place
    {
    }
}
