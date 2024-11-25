namespace WorldOfZuul
{
    public class Room
    {
        public Dictionary<string, string> Interactables { get; set; }= [];
        // it's better for the rooms to know about the objects in them
        // what about this: public List<Interactable> Interactables {get; set; }
        public string ShortDescription { get; set; }="";
        public string LongDescription { get; set;}="";
        public Dictionary<string, string> Exits { get; set; } = [];

        public Game GameRef = null!;

        public Room(string roomId, string shortDescription, string longDescription, Game gameRef)
        {
            ShortDescription = shortDescription;
            LongDescription = longDescription;
            GameRef = gameRef;
            RoomId = roomId;
        }

        public Room(){}

        public string RoomId { get; set; }= "";

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
