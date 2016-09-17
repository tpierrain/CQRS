using System.Linq;
using BookARoom.Infra.Adapters;
using NUnit.Framework;

namespace BookARoom.Tests
{
    [TestFixture]
    public class PlaceCatalogFileAdapterTests
    {
        [Test]
        public void Should_load_a_file()
        {
            var places = new PlaceCatalogFileAdapter(@"../../../../IntegrationFiles/");
            places.LoadPlaceFile("New York Sofitel-availabilities.json");

            Assert.AreEqual(1, places.Places.Count());
        }
    }
}