using System.Linq;
using BookARoom.Infra.ReadModel.Adapters;
using NUnit.Framework;

namespace BookARoom.Tests
{
    [TestFixture]
    public class PlacesAndRoomsAdapterTests
    {
        [Test]
        public void Should_load_a_file()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../IntegrationFiles/");

            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");
            Assert.AreEqual(1, placesAdapter.Places.Count());

            placesAdapter.LoadPlaceFile("THE GRAND BUDAPEST HOTEL-availabilities.json");
            Assert.AreEqual(2, placesAdapter.Places.Count());
        }
    }
}