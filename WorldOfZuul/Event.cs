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
    protected Game? gameRef; // gameRef.Events["Petunia1"].Activate();


    public Event(Resources changeInResources)
    {
        IsActive = false;
        ActivatesAfterFinish = new List<string>();
        ChangeInResources = changeInResources; // resource changes to this event
    }
    public Event(){}
   
    public virtual bool Run() // Method to start the event
    //Virtual so it can be overriden for subclasses(C# magic)
    {
        if (!IsActive)
            return false;

        
        CompleteEvent();
        return true;
    }



    protected void CompleteEvent()
    {
        IsActive = false;
        gameRef.Player.ChangeResources(ChangeInResources); // change resources on the player
        
        foreach (string nextEvent in ActivatesAfterFinish)
        {
            gameRef.Events[nextEvent].IsActive = true;
        }
    }

    public void setupGameRef(Game gr){
        gameRef = gr;
    }
}



public class TextEvent : Event
{   
    public string Text {get; set;} = "";

    public TextEvent(string text, Resources changeInResources) : base(changeInResources){
        Text = text; // Text to be printed when event gets run
    } // Constructor, uses constructor of Event but with added Text var
    
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
    public List<Option>? Options {get; set;} 
    
    
    public QuizEvent(string text, List<Option> options, Resources changeInResources) : base(changeInResources){
        Text = text; // Text to be printed when event gets run
        Options = options;

    }
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


    
public class Option{
    public string OptionText {get; set;} = "";
    public string NextEventName {get; set;} = "";

    public Option(){}


}