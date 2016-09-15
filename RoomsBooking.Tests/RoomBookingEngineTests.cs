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
            var bookingEngine = new RoomBookingEngine(new PlacesCatalog());
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

    public class PlacesCatalog
    {
        private List<Place> places;

        public PlacesCatalog()
        {
            this.places = new List<Place>();
        }

        public PlacesCatalog(List<Place> places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return this.places.FindAll(p => p.Location == location);
        }
    }

    public class RoomBookingEngine
    {
        private readonly PlacesCatalog places;

        public RoomBookingEngine(PlacesCatalog places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchPlaceToStay(DateTime checkInDate, DateTime checkOutDate, string location, int roomNumber, int adultsCout, int childrenCount)
        {
            return this.places.SearchFromLocation(location);
        }
    }

    public class Place
    {
        public string Location { get; }
    }
}
