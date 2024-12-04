using System.Text.Json.Serialization;

namespace WorldOfZuul;
public class Interactable
{
    public string Name {get; set; }="";
    
    public List<string> Events {get; set; }=[]; // Only Events to be Run sequentially, don't put in QuizEvent branches

    public bool IsAvailable {get; set;}=true;

    [JsonIgnore]
    protected Game gameRef=null!; // a funny operator that allows us to initialise this later than the constructor;DD
    public Interactable(Game game, string name)
    {
        gameRef = game;              // so the main game would know what is happening, e.g., so it would know that the player found a key
        Name = name;
        Events = [" "]; // if you don't initialise a list, you can't put anything into it
                                     // we want, e.g., this not to crash: Events.Add(newEvent)
        
    }
    public Interactable(){}

    // do this, we just need to finish the inside of the foreach loop, get the event from the Events dictionary
    // through the gameRef and run it 
    public void Interact()   // this is what happens when the player in the game interacts with the interactable
    {
        Console.WriteLine($"Interacting with {Name}"); // To be removed
        foreach (string current_event_name in Events)
        {       
            Event curr_event = gameRef.Events[current_event_name];
            curr_event.Run();
        }
    }

    public void setupGameRef(Game gr){
        gameRef = gr;
    }
    
}