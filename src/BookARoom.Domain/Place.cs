namespace BookARoom.Domain
{
    public class Place
    {
        public Place(string name, string location)
        {
            this.Name = name;
            this.Location = location;
        }

        public string Location { get; }
        public string Name { get; }
    }
}