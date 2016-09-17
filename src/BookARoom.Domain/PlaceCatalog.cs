namespace BookARoom
{
    using System.Collections.Generic;

    public class PlaceCatalog : ICatalogPlaces
    {
        private List<Place> places;

        public PlaceCatalog()
        {
            this.places = new List<Place>();
        }

        public PlaceCatalog(List<Place> places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return this.places.FindAll(p => p.Location == location);
        }
    }
}