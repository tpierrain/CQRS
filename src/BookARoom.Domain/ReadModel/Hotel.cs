namespace BookARoom.Domain.ReadModel
{
    public class Hotel
    {
        public Hotel(int hotelId, string name, string location, int numberOfRooms)
        {
            this.Identifier = hotelId;
            this.Name = name;
            this.Location = location;
            this.NumberOfRooms = numberOfRooms;
        }

        public string Location { get; }
        public string Name { get; }
        public int Identifier { get; }
        public int NumberOfRooms { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}