namespace WorldOfZuul;
public class Interactable
{
    public string Name {get; set; }
    public Room ParentRoom {get; set; } // npc's aren't going to move around, so the room needs to know about the NPC but not the other way around
                                        // does the npc need to have ParentRoom
    public List<string> Events {get; set; } 
    protected Game gameRef; 
    public Interactable(Game game, string name, Room parentRoom)
    {
        gameRef = game;              // so the main game would know what is happening, e.g., so it would know that the player found a key
        Name = name;
        Events = new List<string>(); // if you don't initialise a list, you can't put anything into it
                                     // we want, e.g., this not to crash: Events.Add(newEvent)
        ParentRoom = parentRoom;
    }

        public void Interact()   // this is what happens when the player in the game interacts with the interactable
        {                        // but how do you trigger this? where do you write for the game to recognise what an interact is or how to activate it?
                                 // then the game needs to find the right interactable and call it's interact method;)
            Console.WriteLine($"Interacting with {Name}"); // To be removed
            foreach (string current_event in Events)
            {
                //current_event.Run();
            }
        }
    
}