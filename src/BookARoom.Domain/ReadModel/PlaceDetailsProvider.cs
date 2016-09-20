using System;

namespace BookARoom.Domain.ReadModel
{
    public class PlaceDetailsProvider
    {
        public IProvidePlacesDetails PlacesDetails { get; }

        public PlaceDetailsProvider(IProvidePlacesDetails placesDetails)
        {
            this.PlacesDetails = placesDetails;
        }

        public PlaceDetails GetDetails(int placeId)
        {
            throw new NotImplementedException();
        }
    }
}