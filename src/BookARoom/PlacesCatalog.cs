namespace BookARoom
{
    using System.Collections.Generic;

    public class PlacesCatalog
    {
        private List<Place> places;

        public PlacesCatalog()
        {
            this.places = new List<Place>();
        }

        public PlacesCatalog(List<Place> places)
        {
            this.places = places;
        }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            return this.places.FindAll(p => p.Location == location);
        }
    }
}