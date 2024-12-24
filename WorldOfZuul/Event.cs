using System.Text.Json.Serialization;

namespace WorldOfZuul;


public class Event
{

    public bool IsActive { get; set; } = true;
    public string? ParentInteractableName {get; set;} // event knows about the interactable
    public List<string> ActivatesAfterFinish { get; set; } = new();// list<string> instead of event, why?
    //public string Description { get; set; } = ""; // Default Val is empty, used for options in QuizEvent and for easier orientation
    public Resources ChangeInResources {get; set;} = new(); // Dictionary to store how resources change when event happens 
    public int MoneyRequired {get; set;} = 0;
    [JsonIgnore]
    protected Game gameRef = null!; // gameRef.Events["Petunia1"].Activate();
    public EventHelper? Helper;


    public Event(){}

    public virtual bool Run() // Method to start the event
    //Virtual so it can be overriden for subclasses(C# magic)
    {
        if (!IsActive)
            return false;

        if(gameRef.Player.WorldStats.Money < MoneyRequired){
            Console.WriteLine("Not enough money");
            return false;
        }
        gameRef.Player.WorldStats.Money -= MoneyRequired;
        CompleteEvent();
        Helper?.Execute(gameRef);

        return true;
    }



    protected void CompleteEvent()
    {
        IsActive = false;
        gameRef.Player.ChangeResources(ChangeInResources); // change resources on the player
        
        foreach (string nextEvent in ActivatesAfterFinish)
        {
            gameRef.Events![nextEvent].IsActive = true;
        }
    }

    public void setupGameRef(Game gr){
        gameRef = gr;
    }
}



public class TextEvent : Event
{   
    public string Text {get; set;} = "";

    public TextEvent(){}

    public override bool Run(){
        
        if(!base.Run()){
            return false;
        }

        Console.WriteLine(Text);
        return true;
    }
}

public class QuizEvent: Event{

    public string Text {get; set;} = "";
    public List<Option> Options {get; set;} =[];
    
    
    public QuizEvent() { }

    public override bool Run(){
        
        if(!base.Run()){
            return false;
        }
        
        Console.WriteLine(Text);
        Option? option;
        
        for(int i = 0; i < Options.Count; i++)
        {
            option = Options[i];
            Console.WriteLine(@$"{(char)(i+'a')}: {option.OptionText}");
        }

        string? input = "";

        while(input == ""){
            input = Console.ReadLine();
            input = input?.ToLower();
            if(input == null || input.Length != 1){
                input = "";
                continue;
                // If input is not a character it starts the loop over again
            }
            int n = input[0] - 'a';
            if(n >= Options.Count || n < 0){
                input = "";
                continue;
            }
            // Apply resource changes for a selected option
            // Each option will contain specific resource changes
            gameRef.Events[Options[n].NextEventName].Run();
        }


        return true;
    }

}

public class MultipleChoiceEvent : Event
{

    public string Text {get; set;} = "";
    
    // how many times to itterate the options list number
    public int OptionsIterations {get; set;}
    public List<string> Options {get; set;} =[];

    public List<bool> CorrectOptions {get; set;} =[];

    public Dictionary<int, string> FollowingEvents {get; set;} = [];

    public MultipleChoiceEvent() { } 
    
    public override bool Run()
    {
        if (!base.Run())
        {
            return false;
        }

        Console.WriteLine(Text);
        int successCounter = 0;
        List<string> remainingOptions = [.. Options];
        List<bool> remainingCorrect = [.. CorrectOptions]; // I don't know this expression, but that's what quick fix did with ...= new List<bool>(CorrectOptions)

    while(OptionsIterations > 0)
    {
        // the same Options display logic as in quiz events; (for...in)
        for (int j = 0; j < remainingOptions.Count; j++)
            {
                Console.WriteLine($"{(char)(j + 'a')}: {remainingOptions[j]}");
            }

            string? input = Console.ReadLine();
            
            if (string.IsNullOrEmpty(input) || input.Length != 1)
            {
                Console.WriteLine("");
                continue;
            }

        int selectedIndex = input[0] - 'a'; // now the input is an index
        if ( selectedIndex < 0 || selectedIndex >= remainingOptions.Count)
            {
                Console.WriteLine("");
                continue;
            }
            
        if (remainingCorrect[selectedIndex])
            {
                successCounter++;
                Console.WriteLine("\nHmm...Boss nods along, he appears to be genuinely intrigued.\n");
                // I know that this is only for the boss quest, but if we're gonna use this event type more, I'll just write "AnswerText" variable that could be dynamic
            }
            else
            {
            Console.WriteLine("\nBoss rolls his eyes, uh oh it seems that just made him more annoyed.\n");
            }

        remainingOptions.RemoveAt(selectedIndex);
        remainingCorrect.RemoveAt(selectedIndex);

        OptionsIterations--;        
    }

    if (FollowingEvents.TryGetValue(successCounter, out var nextEventName))
        {
            gameRef.Events[nextEventName].Run();
        }

    return true;
    } 
}

public class Option{
    public string OptionText {get; set;} = "";
    public string NextEventName {get; set;} = "";

    public Option(){}


}