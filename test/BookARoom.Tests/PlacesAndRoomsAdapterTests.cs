using BookARoom.Infra;
using BookARoom.Infra.MessageBus;
using BookARoom.Infra.ReadModel.Adapters;
using NFluent;
using NUnit.Framework;

namespace BookARoom.Tests
{
    [TestFixture]
    public class PlacesAndRoomsAdapterTests
    {
        [Test]
        public void Should_load_a_file()
        {
            var placesAdapter = new PlacesAndRoomsAdapter(@"../../integration-files/", new FakeBus());

            placesAdapter.LoadPlaceFile("New York Sofitel-availabilities.json");
            Check.That(placesAdapter.Places).HasSize(1);

            placesAdapter.LoadPlaceFile("THE GRAND BUDAPEST HOTEL-availabilities.json");
            Check.That(placesAdapter.Places).HasSize(2);
        }
    }
}