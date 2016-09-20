namespace BookARoom.Domain.ReadModel
{
    public interface IProvidePlacesDetails
    {
        PlaceDetails GetDetails(int placeId);
    }
}