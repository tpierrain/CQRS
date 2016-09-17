using System;
using BookARoom.Infra;
using NUnit.Framework;
using System.Linq;
using BookARoom.Infra.Adapters;

namespace BookARoom.Tests
{
    using BookARoom;

    [TestFixture]
    public partial class RoomBookingEngineTests
    {
        [Test]
        public void Should_find_no_room_when_searching_an_empty_location_catalog()
        {
            var bookingEngine = new RoomBookingEngine(new PlaceCatalog());
            var availablePlaces = bookingEngine.SearchPlaceToStay(checkInDate:DateTime.Now, checkOutDate:DateTime.Now.AddDays(1), location:"Paris", roomNumber:1, adultsCout:2, childrenCount:0);
            Assert.AreEqual(0, availablePlaces.Count());
        }

        [Test]
        public void Should_find_room_when_one_match_and_is_available()
        {
            var places = new PlaceCatalogFileAdaptor(@"../../../../IntegrationFiles/");
            //var placesCatalog = new PlaceCatalog(new List<Place>() { new Place("Paris", "Georges V", "2016-09-20: "), new Place("NY", "Sofitel New York") });
            throw new NotImplementedException();
        }
    }
}
