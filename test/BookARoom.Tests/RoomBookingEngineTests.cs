namespace RoomsBooking.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using BookARoom;

    [TestFixture]
    public class RoomBookingEngineTests
    {
        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var bookingEngine = new BookARoom.RoomBookingEngine(new PlacesCatalog());
            IEnumerable<Place> availablePlaces = bookingEngine.SearchPlaceToStay(checkInDate:DateTime.Now, checkOutDate:DateTime.Now.AddDays(1), location:"Paris", roomNumber:1, adultsCout:2, childrenCount:0);
            Assert.AreEqual(0, availablePlaces.Count());
        }

        [Test]
        public void Should_find_room_when_one_match_and_is_available()
        {
            //var placesCatalog = new PlacesCatalog(new List<Place>() { new Place("Paris", "Georges V", "2016-09-20: "), new Place("NY", "Sofitel New York") });
            throw new NotImplementedException();
        }
    }
}
