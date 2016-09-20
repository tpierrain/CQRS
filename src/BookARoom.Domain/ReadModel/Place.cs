namespace BookARoom.Domain.ReadModel
{
    public class Place
    {
        public Place(int placeId, string name, string location, int numberOfRooms)
        {
            this.Identifier = placeId;
            this.Name = name;
            this.Location = location;
            this.NumberOfRooms = numberOfRooms;
        }

        public string Location { get; }
        public string Name { get; }
        public int Identifier { get; }
        public int NumberOfRooms { get; }
    }
}