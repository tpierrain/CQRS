using BookARoom.Infra.MessageBus;
using BookARoom.Infra.ReadModel.Adapters;
using BookARoom.Tests.Acceptance;
using NFluent;
using NUnit.Framework;

namespace BookARoom.Tests
{
    [TestFixture]
    public class HotelsAndRoomsAdapterTests
    {
        [Test]
        public void Should_load_a_file()
        {
            var hotelsAdapter = new HotelsAndRoomsAdapter(Constants.RelativePathForHotelIntegrationFiles, new FakeBus());

            hotelsAdapter.LoadHotelFile("New York Sofitel-availabilities.json");
            Check.That(hotelsAdapter.Hotels).HasSize(1);

            hotelsAdapter.LoadHotelFile("THE GRAND BUDAPEST HOTEL-availabilities.json");
            Check.That(hotelsAdapter.Hotels).HasSize(2);
        }
    }
}