namespace WorldOfZuul
{
    public class Room
    {
        public Dictionary<string, Interactable> Interactables { get; private set; } = new();
        public string ShortDescription { get; private set; }
        public string LongDescription { get; private set;}
        public Dictionary<string, Room> Exits { get; private set; } = new();

        public Room(string shortDesc, string longDesc, Dictionary<string, Interactable> interactables)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
            Interactables = interactables;
        }

        public void SetExits(Room? north, Room? east, Room? south, Room? west)
        {
            SetExit("north", north);
            SetExit("east", east);
            SetExit("south", south);
            SetExit("west", west);
        }

        public void SetExit(string direction, Room? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }
    }
}
