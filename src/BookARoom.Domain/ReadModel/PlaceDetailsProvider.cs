using System;

namespace BookARoom.Domain.ReadModel
{
    public class PlaceDetailsProvider
    {
        public IProvidePlaces Places { get; }

        public PlaceDetailsProvider(IProvidePlaces places)
        {
            this.Places = places;
        }

        public Place GetDetails(int placeId)
        {
            throw new NotImplementedException();
        }
    }
}