using BookARoom.Domain.ReadModel;
using BookARoom.Infra.Adapters;
using NUnit.Framework;

namespace BookARoom.Tests.Acceptance
{
    [TestFixture]
    public class PlaceDetailsProviderTests
    {
        [Test]
        public void Should_get_place_details()
        {
            var placeId = 1;
            var placeDetailsProvider = new PlaceDetailsProvider(new PlaceCatalogFileAdapter(@"../../IntegrationFiles/"));

            var placeDetails = placeDetailsProvider.GetDetails(placeId: placeId);

            Assert.AreEqual(placeId, placeDetails.Identifier);
            Assert.AreEqual("New York Sofitel", placeDetails.Name);
            Assert.AreEqual("New York", placeDetails.Location);
            Assert.AreEqual(3, placeDetails.NumberOfRooms);
        }
    }
}
