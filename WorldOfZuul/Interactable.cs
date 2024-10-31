namespace WorldOfZuul
{
    public record Interactable(string Name, Room ParentRoom, List<Event> Events)
    {
        protected Game gameRef;
        public Interactable(Game game, string name, Room parentRoom) : this(name, parentRoom, new List<Event>())
        {
            gameRef = game;              // so the main game would know what is happening, e.g., so it would know that the player found a key

            // if you don't initialise a list, you can't put anything into it
            // we want, e.g., this not to crash: Events.Add(newEvent)

        }
namespace WorldOfZuul;

public class Interactable
{
    public string Name {get; set; }
    public Room ParentRoom {get; set; }
    public List<Event> Events {get; set; } 
    protected Game gameRef; 
    public Interactable(Game game, string name, Room parentRoom)
    {
        gameRef = game;              // so the main game would know what is happening, e.g., so it would know that the player found a key
        Name = name;
        Events = new List<Event>(); // if you don't initialise a list, you can't put anything into it
                                     // we want, e.g., this not to crash: Events.Add(newEvent)
        ParentRoom = parentRoom;
    }

        public void Interact()   // this is what happens when the player in the game interacts with the interactable
        {
            Console.WriteLine($"Interacting with {Name}"); // To be removed
            foreach (Event current_event in Events)
            {
                current_event.Run();
            }
        }
    }
}
    public void Interact()   // this is what happens when the player in the game interacts with the interactable
    {
        Console.WriteLine($"Interacting with {Name}"); // To be removed
        foreach (Event current_event in Events)
        {
            current_event.Run();
        }
    }
}

// example child class: