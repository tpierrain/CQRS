using System.Collections.Generic;
using BookARoom.Domain;

namespace BookARoom.Infra.Adapters
{
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