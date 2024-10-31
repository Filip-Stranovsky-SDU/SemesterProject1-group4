namespace WorldOfZuul
{
    public class Room
    {
        public Dictionary<string, string>? Interactables { get; set; }
        public string? ShortDescription { get; private set; }
        public string? LongDescription { get; private set;}
        public Dictionary<string, string>? Exits { get; set; }

        public Game? GameRef;

        /*public Room(string shortDesc, string longDesc, Dictionary<string, Interactable> interactables)
        {
            ShortDescription = shortDesc;
            LongDescription = longDesc;
        }*/

        public void SetExits(string? north, string? east, string? south, string? west)
        {
            SetExit("north", north);
            SetExit("east", east);
            SetExit("south", south);
            SetExit("west", west);
        }

        public void SetExit(string direction, string? neighbor)
        {
            if (neighbor != null)
                Exits[direction] = neighbor;
        }
    }
}
