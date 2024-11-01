namespace WorldOfZuul;


public class Event
{

    public bool IsActive { get; set; }
    public string? ParentInteractableName {get; set;} // event knows about the interactable
    public List<string> ActivatesAfterFinish { get; set; } // list<string> instead of event, why?
    public string Description { get; set; } = ""; // Default Val is empty, used for options in QuizEvent and for easier orientation
    public Dictionary<string, int> ChangeInResources {get; set;} = new(); // Dictionary to store how resources change when event happens 
    protected Game? gameRef; // gameRef.Events["Petunia1"].Activate();


    public Event(string description, Dictionary<string, int> changeInResources)
    {
        IsActive = false;
        ActivatesAfterFinish = new List<string>();
        Description = description;
        ChangeInResources = changeInResources; // resource changes to this event
    }
   
    public virtual bool Run() // Method to start the event
    //Virtual so it can be overriden for subclasses(C# magic)
    {
        if (!IsActive)
            return false;

        gameRef.Player.ChangeResources(ChangeInResources); // change resources on the player
        
        CompleteEvent();
        return true;
    }



    protected void CompleteEvent()
    {
        IsActive = false;
        foreach (string nextEvent in ActivatesAfterFinish)
        {
            gameRef.Events[nextEvent].Run();
        }
    }
}



public class TextEvent : Event
{   
    public string Text {get; set;} = "";

    public TextEvent(string description, string text, Dictionary<string, int> changeInResources) : base(description, changeInResources){
        Text = text; // Text to be printed when event gets run
    } // Constructor, uses constructor of Event but with added Text var
    
    public override bool Run(){
        if (!IsActive)
        {
            return false;
        }
        Console.Write(Text);
        if (gameRef?.Player != null)
        {
            gameRef.Player.ChangeResources(ChangeInResources); // Call ChangeResources on the player
        }
        CompleteEvent();
        return true;
    }
}

public class QuizEvent: Event{

    public string Text {get; set;} = "";
    public List<string> Options {get; set;} = new();
    
    public QuizEvent(string description, 
                    string text, List<string> options, Dictionary<string, int> changeInResources) : base(description, changeInResources){
        Text = text; // Text to be printed when event gets run
        Options = options;

    }
    public override bool Run(){
        if (!IsActive)
        {
            return false;
        }
        Console.Write(Text);
        string option = "";
        for(int i = 0; i < Options.Count; i++)
        {
            option = Options[i];
            Console.WriteLine(@$"{i+'a'}: {gameRef.Events[option].Description}");
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
            gameRef.Events[Options[n]].Run();
        }


        CompleteEvent();
        return true;
    }

}

public class Option
{
    public string Text {get; set;} = "";
    public string NextNode {get; set;} = "";


}

    
    // Option Class to represent individual choices in a QuizEvent
   /* public class Option: Event
    {
        public string Text { get; set; }
        public Dictionary<string, int> ResourceChanges { get; set; } = new();

        public Option(string text, Dictionary<string, int> resourceChanges)
        {
            Text = text;
            ResourceChanges = resourceChanges;
        }

        public void Run()
        {
            Console.WriteLine(Text);

            // Apply resource changes when this option is selected
            if (gameRef?.Player != null)
            {
                gameRef.Player.ChangeResources(ResourceChanges); // Applies changes for this option
            }
        }
}*/

